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
    [CustomAuthorize("professor", "admin","student")]

    public class ResultsController : Controller
    {
        private ExamEntities db = new ExamEntities();

        [CustomAuthorize("admin")]

        // GET: Results
        public ActionResult Index()
        {
            var results = db.Results.Include(r => r.Student).Include(r => r.Subject);
            return View(results.ToList());
        }
        [CustomAuthorize("professor","admin")]
        public ActionResult ResultofS(int id)
        {
            var results = db.Results.Where(m=>m.S_id==id).ToList();
            return View(results);
        }

        [CustomAuthorize("professor", "admin")]
        public ActionResult OpenExam(int id)
        {
            IEnumerable<Result> results = db.Results.Where(m => m.S_id == id).ToList();
            foreach (var item in results)
            {
                item.StartExam = true;
                item.result1 = 0.0;
                item.S_GPA = 0;
                item.grade ="0";

               
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                
            }
            return View(results);
        }

        [CustomAuthorize("professor", "admin")]
        public ActionResult EndExam(int id)
        {
            IEnumerable<Result> results = db.Results.Where(m => m.S_id == id).ToList();
            foreach (var item in results)
            {
                item.StartExam = false;
               


                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();

            }
            return View(results);
        }

        // GET: Results/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
        [CustomAuthorize("professor", "admin", "student")]

        // GET: Results/Create
        public ActionResult Create()
        {

            //  ViewBag.ST_id = new SelectList(db.Students, "ST_id", "name");
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name");
            return View();

        }
        [CustomAuthorize("professor", "admin", "student")]

        // POST: Results/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ST_id,S_id,result1,grade,S_GPA")] Result result)
        {
            var user_id = (int)Session["UserId"];
            var prev_subs = db.Results.Where(u => u.ST_id == user_id).ToList();
            foreach(var i in prev_subs)
            { if (i.S_id==result.S_id)
                    return RedirectToAction("ProfessorSubjects", "Subjects");

            }
            if (ModelState.IsValid)
            {

                result.ST_id= (int)Session["UserId"];
                result.grade = "0";
                result.S_GPA = 0.0;
                result.result1 = 0.0;

                db.Results.Add(result);
                db.SaveChanges();
                
                return RedirectToAction("ProfessorSubjects", "Subjects");
            }

            ViewBag.ST_id = new SelectList(db.Students, "ST_id", "name", result.ST_id);
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", result.S_id);
            return View(result);
        }
        [CustomAuthorize("professor", "admin")]


        // GET: Results/Edit/5
        public ActionResult Edit(int? ST_id,int S_id  )
        {
            if (ST_id == 0 || S_id ==0 )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
       //     var user_id = (int)Session["UserId"];
            Result result = db.Results.Where(u => u.ST_id == ST_id && u.S_id == S_id)
                                       .FirstOrDefault(); if (result == null)
            {
                return HttpNotFound();
            }
            ViewBag.ST_id = new SelectList(db.Students, "ST_id", "name", result.ST_id);
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", result.S_id);
            return View(result);
        }

        // POST: Results/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [CustomAuthorize("professor", "admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ST_id,S_id,result1,grade,S_GPA")] Result result)
        {
            if(result.result1<50)
            {
                result.S_GPA = 0.0;
                result.grade = "F";
            }
           else if (result.result1 >= 50 && result.result1 < 60)
            {
                result.S_GPA = 1.7;
                result.grade = "D";
            }
           else if (result.result1 >= 60 && result.result1 < 65)
            {
                result.S_GPA = 2.0;
                result.grade = "D+";
            }
           else if (result.result1 >= 65 && result.result1 < 70)
            {
                result.S_GPA = 2.4;
                result.grade = "C";
            }
            else if (result.result1 >= 70 && result.result1 < 75)
            {
                result.S_GPA = 2.7;
                result.grade = "C+";
            }
           else if (result.result1 >= 75 && result.result1 < 80)
            {
                result.S_GPA = 3.0;
                result.grade = "B";
            }

          else  if (result.result1 >= 80 && result.result1 < 85)
            {
                result.S_GPA = 3.3;
                result.grade = "B+";
            }
            else if (result.result1 >= 85 && result.result1 < 90)
            {
                result.S_GPA = 3.7;
                result.grade = "A";
            }
           else if (result.result1 >= 90)
            {
                result.S_GPA = 4.0;
                result.grade = "A+";
            }




            if (ModelState.IsValid)
            {
                db.Entry(result).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProfessorSubjects", "Subjects");
            }
            ViewBag.ST_id = new SelectList(db.Students, "ST_id", "name", result.ST_id);
            ViewBag.S_id = new SelectList(db.Subjects, "S_id", "name", result.S_id);
            return View(result);
        }
        [CustomAuthorize( "admin")]

        // GET: Results/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Result result = db.Results.Find(id);
            if (result == null)
            {
                return HttpNotFound();
            }
            return View(result);
        }
        [CustomAuthorize("admin")]

        // POST: Results/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Result result = db.Results.Find(id);
            db.Results.Remove(result);
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
