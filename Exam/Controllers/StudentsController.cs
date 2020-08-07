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
    [CustomAuthorize("student","admin","professor")]

    public class StudentsController : Controller
    {

        private ExamEntities db = new ExamEntities();

        [CustomAuthorize("student")]
        [HttpGet]
        public ActionResult StudentPage()
        {
            var user_id = (int)Session["UserId"];
            Student student = db.Students.SingleOrDefault(m => m.ST_id == user_id);
            if (student.ST_image == null)
            {
                return View("StudentPagenoph", student);
            }
            return View(student);

        }
        [CustomAuthorize("student")]
        public ActionResult StudentResult()
        {
            var user_id = (int)Session["UserId"];
            var st = db.Students.Single(m => m.ST_id == user_id);
            var st_sub = db.Results.Where(m => m.ST_id == user_id).ToList();
            ViewBag.S_id = new SelectList(db.Subjects.Where(m=>m.L_id==st.L_id), "S_id", "name");

            return View(st_sub);
        }

        // GET: Students
        [CustomAuthorize("admin")]

        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department).Include(s => s.Level).Where(v => v.approval == true);
            return View(students.ToList());
        }


        [CustomAuthorize("admin")]
        public ActionResult RequestStudents()
        {
            var b = db.Students.Where(v => v.approval == false).ToList();
            return View("RequestStudents", b);
        }
        [CustomAuthorize("admin")]

        public ActionResult Approve(int id)
        {
            var user = db.Students.SingleOrDefault(m => m.ST_id == id);
            user.approval = true;
            db.SaveChanges();
            return RedirectToAction("RequestStudents","Students");

        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        [CustomAuthorize("admin")]

        public ActionResult Create()
        {
            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name");
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "ST_id,name,email,passwod,mobile,N_N,rule,approval,L_id,Dep_id,total,ST_image")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name", student.Dep_id);
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id", student.L_id);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name", student.Dep_id);
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id", student.L_id);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ST_id,name,email,passwod,mobile,N_N,rule,approval,L_id,Dep_id,total,ST_image")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name", student.Dep_id);
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id", student.L_id);
            return View(student);
        }
        [CustomAuthorize("admin")]

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
