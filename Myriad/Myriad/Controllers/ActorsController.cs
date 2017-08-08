using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Myriad.Models;

namespace Myriad.Controllers
{
    public class ActorsController : Controller
    {
        private MyriadDbEntities db = new MyriadDbEntities();

        // GET: Actors
        public ActionResult Index()
        {
            return View(db.Actors.ToList());
        }

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }


        public PartialViewResult DetailsPartial(int id)
        {
            Actor actor = db.Actors.Find(id);
            return PartialView(actor);
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            List<SelectListItem> genderList = new List<SelectListItem>();
            genderList.Add(new SelectListItem { Text = "Male", Value = "1" });
            genderList.Add(new SelectListItem { Text = "Female", Value = "2" });
            genderList.Add(new SelectListItem { Text = "Others", Value = "3" });

            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActID,Name,Sex,DOB,Bio")] Actor actor)
        {
            
            if (ModelState.IsValid)
            {
                db.Actors.Add(actor);
                db.SaveChanges();
                return RedirectToAction("CreateAllAction","Movies");
            }

            return View(actor);
        }

        public PartialViewResult CreateActorPartialView()
        {
            ActorsViewModels actor = new ActorsViewModels();
            Movie movie = new Movie();
            ViewBag.ProID = new SelectList(db.Producers, "ProID", "Name", movie.ProID);
            ViewBag.Sex = new SelectList(MoviesController.GetGender(), "Value", "Text", actor.Sex);
            return PartialView("CreateActorPartialView");
        }

        [HttpPost]
        //,ActionName("CreateActor")
        public ActionResult CreateActorPartialView(ActorsViewModels actorModel)
        {

            if (ModelState.IsValid)
            {
                Actor actor = new Actor();
                actor.Name = actorModel.Name;
                actor.Sex = (byte)actorModel.Sex;
                actor.Bio = actorModel.Bio;
                actor.DOB = actorModel.DOB;
                actor.MovieActors = actorModel.MovieActors;
                db.Actors.Add(actor);
                db.SaveChanges();

                return Content("Actor Added Successfully");
            }
            ViewBag.Sex = new SelectList(MoviesController.GetGender(), "Value", "Text", actorModel.Sex);
            return PartialView(actorModel);
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActID,Name,Sex,DOB,Bio")] Actor actor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(actor);
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Actor actor = db.Actors.Find(id);
            if (actor == null)
            {
                return HttpNotFound();
            }
            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Actor actor = db.Actors.Find(id);
            db.Actors.Remove(actor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CheckActorsPartialView(MovieViewModel m)
        {
            var actorsList = new List<CheckActorsModel>();
            MoviesController mc = new MoviesController();
            actorsList = mc.GetActorsCheckList();
            return PartialView(actorsList);
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
