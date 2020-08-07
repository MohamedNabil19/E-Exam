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
    [CustomAuthorize("admin","professor")]
    public class ExamQuestionsController : Controller
    {
        private ExamEntities db = new ExamEntities();
        [CustomAuthorize("admin")]

        // GET: ExamQuestions
        public ActionResult Index()
        {
            var examQuestions = db.ExamQuestions.Include(e => e.Subject);
            return View(examQuestions.ToList());
        }


        public ActionResult Sub_Exam(int id)
        {
            var examQuestions = db.ExamQuestions.Include(e => e.Subject).Where(d=>d.S_id==id).ToList();
            ViewBag.Sub_id = id;
            var sub = db.Subjects.Find(id);
            ViewBag.Sub_name = sub.name;
            return View(examQuestions);
        }
        public ActionResult StartExam(int id)
        {
            var user = db.ExamQuestions.SingleOrDefault(m => m.E_id == id);
            user.StausPost = true;
            db.SaveChanges();
            return RedirectToAction("ProfessorSubjects", "Subjects");

        }
        public ActionResult StopExam(int id)
        {
            var user = db.ExamQuestions.SingleOrDefault(m => m.E_id == id);
            user.StausPost = false;
            db.SaveChanges();
            return RedirectToAction("ProfessorSubjects", "Subjects");

        }


        // GET: ExamQuestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            return View(examQuestion);
        }

        // GET: ExamQuestions/Create
        public ActionResult Create(int id)
        {
            int userid = (int)Session["UserId"];
           ViewBag.S_id = new SelectList(db.Subjects.Where(r => r.S_id == id), "S_id", "name");
            return View();
        }

        // POST: ExamQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "E_id,E_name,S_id,duration")] ExamQuestion examQuestion)
        {
        
            if (ModelState.IsValid)
            {

                db.ExamQuestions.Add(examQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", examQuestion.S_id);
            return View(examQuestion);
        }

        // GET: ExamQuestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", examQuestion.S_id);
            return View(examQuestion);
        }

       [CustomAuthorize("professor","admin")]
        // GET: ExamQuestions/AddExam
        public ActionResult AddExam()
        {
            int userid = (int)Session["UserId"];
            string role = (string)Session["rule"];
            if (role == "professor")
            { ViewBag.S_id = new SelectList(db.Subjects.Where(r => r.P_id == userid), "S_id", "name"); }
            else
            { ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name"); }

            return View();
        }
/*
        // POST: ExamQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "E_id,E_name,S_id,duration")] ExamQuestion examQuestion)
        {
            if (ModelState.IsValid)
            {
                db.ExamQuestions.Add(examQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", examQuestion.S_id);
            return View(examQuestion);
        }
        */
        // GET: ExamQuestions/Edit/5
       
        // POST: ExamQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "E_id,E_name,S_id,duration")] ExamQuestion examQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(examQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", examQuestion.S_id);
            return View(examQuestion);
        }

        // GET: ExamQuestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            if (examQuestion == null)
            {
                return HttpNotFound();
            }
            return View(examQuestion);
        }

        // POST: ExamQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExamQuestion examQuestion = db.ExamQuestions.Find(id);
            db.ExamQuestions.Remove(examQuestion);
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
