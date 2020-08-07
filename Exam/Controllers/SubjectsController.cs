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
    [CustomAuthorize("admin", "professor")]
    public class SubjectsController : Controller
    {
        private ExamEntities db = new ExamEntities();
       
        // GET: Subjects
        [CustomAuthorize("admin")]
        public ActionResult Index()
        {
            string role = (string)Session["rule"];
            int userid = (int)Session["UserId"];
          
                var subjects = db.Subjects.Include(m => m.Professor).Include(n => n.Chapters).Include(o => o.Department).ToList();
                return View(subjects);

            
        }

        // GET: Subjects
        [CustomAuthorize("professor","admin")]
        public ActionResult ProfessorSubjects()
        {
            string role = (string)Session["rule"];
            int userid = (int)Session["UserId"];

            if (role == "professor")
            {
                var subjects = db.Subjects.Include(m => m.Professor).Where(m => m.P_id == userid).ToList();
                return View(subjects);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }


        [CustomAuthorize("admin", "professor")]

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }
        [CustomAuthorize("admin")]
        // GET: Subjects/Create
        public ActionResult Create()
        {
            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name");
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id");
            ViewBag.P_id = new SelectList(db.Professors, "P_id", "name");
            return View();
        }
        [CustomAuthorize("admin")]

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "S_id,name,P_id,L_id,Dep_id,hours")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Subjects.Add(subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name", subject.Dep_id);
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id", subject.L_id);
            ViewBag.P_id = new SelectList(db.Professors, "P_id", "name", subject.P_id);
            return View(subject);
        }
        [CustomAuthorize("admin")]

        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name", subject.Dep_id);
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id", subject.L_id);
            ViewBag.P_id = new SelectList(db.Professors, "P_id", "name", subject.P_id);
            return View(subject);
        }
        [CustomAuthorize("admin")]

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "S_id,name,P_id,L_id,Dep_id,hours")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name", subject.Dep_id);
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id", subject.L_id);
            ViewBag.P_id = new SelectList(db.Professors, "P_id", "name", subject.P_id);
            return View(subject);
        }
        [CustomAuthorize("admin")]

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }
        [CustomAuthorize("admin")]

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
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
