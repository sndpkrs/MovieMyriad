﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myriad.Models;
using System.IO;
using Amazon.S3;
using Amazon.S3.Model;
using System.Configuration;
using System.Threading;
using Myriad.CustomHelpers;

namespace Myriad.Controllers
{
    public class MoviesController : Controller
    {
        private static readonly string _awsAccessKey = ConfigurationManager.AppSettings["AccessId"];

        private static readonly string _awsSecretKey = ConfigurationManager.AppSettings["secretKey"];

        private static readonly string _bucketName = ConfigurationManager.AppSettings["bucketname"];
        private MyriadDbEntities db = new MyriadDbEntities();

        public static IList<SelectListItem> GetGender()
        {
            IList<SelectListItem> _result = new List<SelectListItem>();
            _result.Add(new SelectListItem { Value = "1", Text = "Male" });
            _result.Add(new SelectListItem { Value = "2", Text = "Female" });
            _result.Add(new SelectListItem { Value = "3", Text = "Others" });
            return _result;
        }

        public string UploadAndRetreieveUrl(HttpPostedFileBase file)
        {
            var additive = DateTime.Now.Millisecond.ToString();
            string keyname = Path.GetFileNameWithoutExtension(file.FileName);

            try
            {
                IAmazonS3 client;
                using (client = Amazon.AWSClientFactory.CreateAmazonS3Client(_awsAccessKey, _awsSecretKey))
                {
                    var request = new PutObjectRequest()
                    {
                        BucketName = _bucketName,
                        CannedACL = S3CannedACL.PublicRead,//PERMISSION TO FILE PUBLIC ACCESIBLE
                        //Key = string.Format("UPLOADS/{0}", file.FileName),
                        Key = string.Format("UPLOADS/{0}", keyname + additive + ".jpg"),
                        InputStream = file.InputStream//SEND THE FILE STREAM
                    };

                    client.PutObject(request);
                }
            }
            catch (Exception e)
            {
                
            }
            string ImageUrl = "https://" + "s3-us-west-2.amazonaws.com/myriadposterappharbour/UPLOADS/" + keyname + additive + ".jpg";
            return ImageUrl;
        }

        public List<CheckActorsModel> GetActorsCheckList()
        {
            var results = from a in db.Actors
                          select new
                          {
                              a.ActID,
                              a.Name,
                              Checked = false
                          };

            var actorsList = new List<CheckActorsModel>();
            try
            {
                foreach (var item in results)
                {
                    actorsList.Add(new CheckActorsModel { id = item.ActID, Name = item.Name, isChecked = item.Checked });
                }
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                ViewBag.error = "We are experiencing huge traffic now";
                
            }
            catch (System.Data.SqlClient.SqlException)
            {
                ViewBag.error = "Your network is a bit slow to catch up";
            }
            
            return actorsList;
        }

        [OutputCache(Duration = 10)]
        // GET: Movies
        public ActionResult Index()
        {
        
            var movies = db.Movies.Include(m => m.Producer);
            var mvModel = new MovieViewModel();
            var mvModelList = new List<MovieViewModel>();

            try
            {

                using (MyriadDbEntities db1= new MyriadDbEntities())
                {
                    db1.Database.Connection.Open();
                    foreach (var item in movies)
                    {
                        var results = from a in db1.Actors
                                      select new
                                      {
                                          a.ActID,
                                          a.Name,
                                          Checked = ((from ab in db1.MovieActors
                                                      where ((item.MovID == ab.MovID) & (a.ActID == ab.ActID))
                                                      select ab).Count() > 0)

                                      };
                        var actorsList = new List<CheckActorsModel>();

                        foreach (var item1 in results)
                        {
                            actorsList.Add(new CheckActorsModel { id = item1.ActID, Name = item1.Name, isChecked = item1.Checked });
                        }

                        mvModelList.Add(new MovieViewModel
                        {
                            MovID = item.MovID,
                            Name = item.Name,
                            Producer = item.Producer,
                            Plot = item.Plot,
                            Poster = item.Poster,
                            ProID = item.ProID,
                            ReleaseDate = item.ReleaseDate,
                            ActorsList = actorsList
                        });
                    } 
                }
            }
            catch (System.Data.Entity.Core.EntityException )
            {
                ViewBag.error = "We are experiencing huge traffic now";
                return View("DbInstanceError");
            }
            catch (System.Data.SqlClient.SqlException )
            {
                ViewBag.error = "Your network is a bit slow to catch up";
                return View("DbInstanceError");
            }
            
            return View(mvModelList);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            var results = from a in db.Actors
                          select new
                          {
                              a.ActID,
                              a.Name,
                              Checked = ((from ab in db.MovieActors
                                          where ((id == ab.MovID) & (a.ActID == ab.ActID))
                                          select ab).Count() > 0)

                          };

            var moviewView = new MovieViewModel();
            moviewView.MovID = id.Value;
            moviewView.Name = movie.Name;
            moviewView.Plot = movie.Plot;
            moviewView.Poster = movie.Poster;
            moviewView.ProID = movie.ProID;
            moviewView.ReleaseDate = movie.ReleaseDate;
            moviewView.Producer = movie.Producer;

            var actorsList = new List<CheckActorsModel>();

            foreach (var item in results)
            {
                actorsList.Add(new CheckActorsModel { id = item.ActID, Name = item.Name, isChecked = item.Checked });

            }
            moviewView.ActorsList = actorsList;
            ViewBag.ProID = new SelectList(db.Producers, "ProID", "Name", movie.ProID);
            return View(moviewView);
        }


        public PartialViewResult DetailsPartial(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                ViewBag.status = "Bad Request";
                return PartialView("EditError.cshtml");
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                ViewBag.status = "This movie does not exist in the catalogue";
                //return HttpNotFound();
                return PartialView("EditError.cshtml");
            }
            return PartialView(movie);
        }


        // GET: Movies/Edit/5
        public PartialViewResult EditPartial(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                ViewBag.status = "Bad Request";
                return PartialView("EditError.cshtml");
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                ViewBag.status = "This movie does not exist in the catalogue";
                //return HttpNotFound();
                return PartialView("EditError.cshtml");
            }

            var results = from a in db.Actors
                          select new
                          {
                              a.ActID,
                              a.Name,
                              Checked = ((from ab in db.MovieActors
                                          where ((id == ab.MovID) & (a.ActID == ab.ActID))
                                          select ab).Count() > 0)

                          };

            var moviewView = new MovieViewModel();
            moviewView.MovID = id.Value;
            moviewView.Name = movie.Name;
            moviewView.Plot = movie.Plot;
            moviewView.Poster = movie.Poster;
            moviewView.ProID = movie.ProID;
            moviewView.ReleaseDate = movie.ReleaseDate;

            var actorsList = new List<CheckActorsModel>();

            foreach (var item in results)
            {
                actorsList.Add(new CheckActorsModel { id = item.ActID, Name = item.Name, isChecked = item.Checked });

            }
            moviewView.ActorsList = actorsList;
            ViewBag.ProID = new SelectList(db.Producers, "ProID", "Name", movie.ProID);
            return PartialView(moviewView);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartial(MovieViewModel mvModel, HttpPostedFileBase file)
        {

            var movie = db.Movies.Find(mvModel.MovID);
            movie.Name = mvModel.Name;
            movie.Plot = mvModel.Plot;
            movie.Poster = mvModel.Poster;
            movie.ReleaseDate = mvModel.ReleaseDate;
            movie.ProID = mvModel.ProID;
            if (ModelState.IsValid)
            {
                
                foreach (var item in db.MovieActors)
                {
                    if (item.MovID == movie.MovID)
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                }

                foreach (var item in mvModel.ActorsList)
                {
                    if (item.isChecked)
                        db.MovieActors.Add(new MovieActor() { MovID = movie.MovID, ActID = item.id });
                }

                db.SaveChanges();
                return Content(mvModel.Name + " details sucessfully updated");
            }
            ViewBag.ProID = new SelectList(db.Producers, "ProID", "Name", movie.ProID);
            return PartialView(mvModel);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id,int? log)
        {
            string returnUrL = Url.Content("/Movies/Delete/" + id);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //if (log == null)
            //    return RedirectToAction("MyriadLogin", "Account");
            //else
            //{
                Movie movie = db.Movies.Find(id);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                if (Request.IsAjaxRequest())
                    return PartialView(movie);
                return View(movie);
            //}
                
            
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            IEnumerable<MovieActor> movieActor = db.MovieActors.Where(m => m.MovID == id);
            foreach (var item in movieActor)
            {
                db.MovieActors.Remove(item);
            }

            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        //[ActionName("CreateCatalogue")]
        [OutputCache(Duration = 5)]
        public ActionResult CreateAllAction()
        {
            var movieModel = new MovieViewModel();
            
            movieModel.ActorsList = GetActorsCheckList();
            ViewBag.ProID = new SelectList(db.Producers, "ProID", "Name", movieModel.ProID);

            return View(movieModel);
        }

        

        [HttpPost]
        //[ActionName("CreateCatalogue")]
        public ActionResult CreateAllAction(MovieViewModel mvModel,int? ProID, List<CheckActorsModel> caModel, HttpPostedFileBase file)
        {
            var movie = new Movie();
            movie.Name = mvModel.Name;
            movie.Plot = mvModel.Plot;
            movie.Producer = mvModel.Producer;
            movie.ReleaseDate = mvModel.ReleaseDate;
            movie.ProID = mvModel.ProID = ProID;
            mvModel.ActorsList = caModel;
            if (ModelState.IsValid)
            {
                if (file != null && (Path.GetFileNameWithoutExtension(file.FileName)==".jpg"
                                     || Path.GetFileNameWithoutExtension(file.FileName) == ".JPG"
                                     || Path.GetFileNameWithoutExtension(file.FileName) == ".jpeg"
                                     || Path.GetFileNameWithoutExtension(file.FileName) == ".JPEG"))
                    movie.Poster = UploadAndRetreieveUrl(file);

                db.Movies.Add(movie);
                if (mvModel.ActorsList != null)
                {
                    foreach (var item in mvModel.ActorsList)
                    {
                        if (item.isChecked)
                            db.MovieActors.Add(new MovieActor() { MovID = movie.MovID, ActID = item.id });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            ViewBag.ProID = new SelectList(db.Producers, "ProID", "Name", movie.ProID);

            return View(mvModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
