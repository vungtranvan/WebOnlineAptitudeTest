using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CategoryExamsController : Controller
    {
        private OnlineTestDbContext db = new OnlineTestDbContext();

        // GET: Admin/CategoryExams
        public ActionResult Index()
        {
            return View(db.CategoryExams.ToList());
        }

        // GET: Admin/CategoryExams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryExam categoryExam = db.CategoryExams.Find(id);
            if (categoryExam == null)
            {
                return HttpNotFound();
            }
            return View(categoryExam);
        }

        // GET: Admin/CategoryExams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/CategoryExams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] CategoryExam categoryExam)
        {
            if (ModelState.IsValid)
            {
                db.CategoryExams.Add(categoryExam);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoryExam);
        }

        // GET: Admin/CategoryExams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryExam categoryExam = db.CategoryExams.Find(id);
            if (categoryExam == null)
            {
                return HttpNotFound();
            }
            return View(categoryExam);
        }

        // POST: Admin/CategoryExams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] CategoryExam categoryExam)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryExam).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoryExam);
        }

        // GET: Admin/CategoryExams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryExam categoryExam = db.CategoryExams.Find(id);
            if (categoryExam == null)
            {
                return HttpNotFound();
            }
            return View(categoryExam);
        }

        // POST: Admin/CategoryExams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoryExam categoryExam = db.CategoryExams.Find(id);
            db.CategoryExams.Remove(categoryExam);
            db.SaveChanges();
            return RedirectToAction("Index");
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
