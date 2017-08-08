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
    public class ProducersController : Controller
    {
        private MyriadDbEntities db = new MyriadDbEntities();

        // GET: Producers
        public ActionResult Index()
        {
            return View(db.Producers.ToList());
        }

        // GET: Producers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = db.Producers.Find(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        public PartialViewResult DetailsPartial(int id)
        {
            Producer producer = db.Producers.Find(id);
            return PartialView(producer);
        }
        // GET: Producers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProID,Name,Sex,DOB,Bio")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                db.Producers.Add(producer);
                db.SaveChanges();
                return RedirectToAction("CreateAll","Movies");
            }

            return View(producer);
        }

        //[ActionName("CreateProducer")]
        public PartialViewResult CreateProducerPartialView()
        {
            Producer pro = new Producer();
            ViewBag.Sex = new SelectList(MoviesController.GetGender(), "Value", "Text", pro.Sex);

            return PartialView("CreateProducerPartialView");
        }

        [HttpPost]
        //[ActionName("CreateProducer")]
        public ActionResult CreateProducerPartialView(ProducerViewModel producerModel)
        {
            if (ModelState.IsValid)
            {
                Producer producer = new Producer();
                producer.Name = producerModel.Name;
                producer.Sex = (byte)producerModel.Sex;
                producer.Bio = producerModel.Bio;
                producer.DOB = producerModel.DOB;

                db.Producers.Add(producer);
                db.SaveChanges();

                return Content("Producer Added Successfully");
            }
            ViewBag.Sex = new SelectList(MoviesController.GetGender(), "Value", "Text", producerModel.Sex);
            return PartialView(producerModel);
        }

        // GET: Producers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = db.Producers.Find(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProID,Name,Sex,DOB,Bio")] Producer producer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producer);
        }

        // GET: Producers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producer producer = db.Producers.Find(id);
            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producer producer = db.Producers.Find(id);
            db.Producers.Remove(producer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult CheckProducerPartialView()
        {
            ProducerViewModel model = new ProducerViewModel();
            ViewBag.ProID = new SelectList(db.Producers, "ProID", "Name", model.ProID);
            return PartialView();
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
