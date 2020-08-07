using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Exam.Models;
using Exam.Infrastructure;
namespace Exam.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("professor","admin")]

    public class ChaptersController : Controller
    {

        private ExamEntities db = new ExamEntities();
        [CustomAuthorize("admin")]
        // GET: Chapters
        public ActionResult Index()
        {
            var chapters = db.Chapters.Include(c => c.Subject);
            return View(chapters.ToList());
        }
        public ActionResult ChaptersofS(int id)
        {
            var chapters = db.Chapters.Include(c => c.Subject).Where(z=>z.S_id==id).ToList();
            ViewBag.N = id;
            return View(chapters);
        }




        // GET: Chapters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // GET: Chapters/Create
        public ActionResult Create(int id)
        {

           var d = db.Subjects.SingleOrDefault(m => m.S_id == id);
         //   ViewBag.S_name = d.name;
            Chapter chap =new Chapter();
            chap.Subject = d;
            chap.S_id = id;
         //   ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name");
            return View(chap);
        }


        // POST: Chapters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CH_id,name,S_id")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Chapters.Add(chapter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", chapter.S_id);
            return View(chapter);
        }

        // GET: Chapters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", chapter.S_id);
            return View(chapter);
        }

        // POST: Chapters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CH_id,name,S_id")] Chapter chapter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chapter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProfessorSubjects", "Subjects");
            }
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", chapter.S_id);
            return View(chapter);
        }

        // GET: Chapters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chapter chapter = db.Chapters.Find(id);
            if (chapter == null)
            {
                return HttpNotFound();
            }
            return View(chapter);
        }

        // POST: Chapters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chapter chapter = db.Chapters.Find(id);
            db.Chapters.Remove(chapter);
            db.SaveChanges();
            return RedirectToAction("ProfessorSubjects", "Subjects");
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
