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
    public class ProfessorsController : Controller
    {
        private ExamEntities db = new ExamEntities();
        [CustomAuthorize("admin")]

        // GET: Professors
        public ActionResult Index()
        {
            var prof = db.Professors.Where(v => v.approval == true).ToList();

            return View(prof);
        }
        [CustomAuthorize("admin")]

        public ActionResult RequestProfessors()
        {
            var b = db.Professors.Where(v => v.approval == false).ToList();
            return View("RequestProfessors", b);
        }

        public ActionResult Approve(int id)
        {
            var user = db.Professors.SingleOrDefault(m => m.P_id == id);
            user.approval = true;
            db.SaveChanges();
            return RedirectToAction("RequestProfessors", "Professors");

        }




        [CustomAuthorize("professor")]
        [HttpGet]
        public ActionResult ProfessorPage()
        {
            var user_id = (int)Session["UserId"];
            Professor prof = db.Professors.SingleOrDefault(m => m.P_id == user_id);
            if (prof.P_image == null)
            {
                return View("ProfessorPagePagenoph", prof);
            }
            return View(prof);
        }


















        // GET: Professors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // GET: Professors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Professors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "P_id,name,emai,password,mobile,N_N,rule,approval,P_image")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                db.Professors.Add(professor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(professor);
        }

        // GET: Professors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // POST: Professors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "P_id,name,emai,password,mobile,N_N,rule,approval,P_image")] Professor professor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(professor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(professor);
        }

        // GET: Professors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Professor professor = db.Professors.Find(id);
            if (professor == null)
            {
                return HttpNotFound();
            }
            return View(professor);
        }

        // POST: Professors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Professor professor = db.Professors.Find(id);
            db.Professors.Remove(professor);
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
