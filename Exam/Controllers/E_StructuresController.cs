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
using System.Web.Routing;

namespace Exam.Controllers
{
    [CustomAuthenticationFilter]
    [CustomAuthorize("admin","professor")]
    public class E_StructuresController : Controller
    {
        private ExamEntities db = new ExamEntities();
        [CustomAuthorize("admin")]
        // GET: E_Structures
        public ActionResult Index()
        {
            var e_Structures = db.E_Structures.Include(e => e.Chapter).Include(e => e.ExamQuestion);
            return View(e_Structures.ToList());
        }
        public ActionResult ShowExam(int id)
        {
            var e_Structures = db.E_Structures.Include(e => e.Chapter).Include(e => e.ExamQuestion).Where(f=>f.E_id==id);
            var subject_e = db.ExamQuestions.Find(id);
            ViewBag.Sub_id = subject_e.S_id;
            ViewBag.Exam_id = id;
            ViewBag.Sub_name = subject_e.Subject.name;
            return View(e_Structures.ToList());
        }



        // GET: E_Structures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            E_Structures e_Structures = db.E_Structures.Find(id);
            if (e_Structures == null)
            {
                return HttpNotFound();
            }
            var sub = db.Subjects.Find(e_Structures.S_id);
            ViewBag.subname = sub.name;
            if (e_Structures.Type_Q == true)
            {
                ViewBag.type = "True or False";
            }
            else
            { ViewBag.type = "MCQ"; }
            return View(e_Structures);
        }

        // GET: E_Structures/Create
        public ActionResult Create(int id,int Exam_id)
        {
            int userid = (int)Session["UserId"];

            ViewBag.CH_id = new SelectList(db.Chapters.Where(d=>d.S_id==id), "CH_id", "name");
            ViewBag.E_id = new SelectList(db.ExamQuestions, "E_id", "E_name");
            //  var Exam = db.ExamQuestions.Find(Exam_id);
            //  ViewBag.Exam_name = Exam.E_name;
            var E_struc = new E_Structures();
            E_struc.E_id = Exam_id;
            E_struc.S_id = id;
           var sub= db.Subjects.Find(id);
            ViewBag.SubjectName = (string)sub.name;
            List<SelectListItem> li_def = new List<SelectListItem>();
            li_def.Add(new SelectListItem { Text = "A", Value = "A" });
            li_def.Add(new SelectListItem { Text = "B", Value = "B" });
            li_def.Add(new SelectListItem { Text = "C", Value = "C" });
            li_def.Add(new SelectListItem { Text = "D", Value = "D" });

            ViewBag.list_def = li_def;

            List<SelectListItem> li = new List<SelectListItem>();
            //   liTypeofQ.Add(new SelectListItem { Text = "Select Type of Question", Value = "0" });
            li.Add(new SelectListItem { Text = "MCQ", Value = "False" });
            li.Add(new SelectListItem { Text = "TrueorFalse", Value = "True" });

            ViewBag.listq = li;
            return View(E_struc);
        }

        // POST: E_Structures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "E_S_id,Type_Q,S_id,def_level,NumofQ,E_id,CH_id,post")] E_Structures e_Structures)
        {
            if (ModelState.IsValid)
            {
                db.E_Structures.Add(e_Structures);
                db.SaveChanges();
                return RedirectToAction("ShowExam", "E_Structures", new { @id = e_Structures.E_id });
            }

            ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", e_Structures.CH_id);
            ViewBag.E_id = new SelectList(db.ExamQuestions, "E_id", "E_name", e_Structures.E_id);
            return View(e_Structures);
        }

        // GET: E_Structures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            E_Structures e_Structures = db.E_Structures.Find(id);
            if (e_Structures == null)
            {
                return HttpNotFound();
            }
            ViewBag.CH_id = new SelectList(db.Chapters.Where(s=>s.S_id==e_Structures.S_id), "CH_id", "name", e_Structures.CH_id);
            ViewBag.E_id = new SelectList(db.ExamQuestions, "E_id", "E_name", e_Structures.E_id);
            ViewBag.SubjectName = (string)e_Structures.Chapter.Subject.name;
            List<SelectListItem> li_def = new List<SelectListItem>();
            li_def.Add(new SelectListItem { Text = "A", Value = "A" });
            li_def.Add(new SelectListItem { Text = "B", Value = "B" });
            li_def.Add(new SelectListItem { Text = "C", Value = "C" });
            li_def.Add(new SelectListItem { Text = "D", Value = "D" });

            ViewBag.list_def = li_def;

             List<SelectListItem> li = new List<SelectListItem>();
         //   liTypeofQ.Add(new SelectListItem { Text = "Select Type of Question", Value = "0" });
            li.Add(new SelectListItem { Text = "MCQ", Value = "False" });
            li.Add(new SelectListItem { Text = "TrueorFalse", Value = "True" });
          
            ViewBag.listq = li;
            return View(e_Structures);
        }

        // POST: E_Structures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "E_S_id,Type_Q,S_id,def_level,NumofQ,E_id,CH_id,post")] E_Structures e_Structures)
        {
            if (ModelState.IsValid)
            {
                db.Entry(e_Structures).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShowExam", "E_Structures", new { @id = e_Structures.E_id });
            }
            ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", e_Structures.CH_id);
            ViewBag.E_id = new SelectList(db.ExamQuestions, "E_id", "E_name", e_Structures.E_id);
            return View(e_Structures);
        }

        // GET: E_Structures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            E_Structures e_Structures = db.E_Structures.Find(id);
            if (e_Structures == null)
            {
                return HttpNotFound();
            }
            var sub = db.Subjects.Find(e_Structures.S_id);
            ViewBag.subname = sub.name;
            if (e_Structures.Type_Q == true)
            {
                ViewBag.type = "True or False";
            }
            else
            { ViewBag.type = "MCQ"; }
            return View(e_Structures);
        }

        // POST: E_Structures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            E_Structures e_Structures = db.E_Structures.Find(id);
            int d = (int)e_Structures.E_id;
            db.E_Structures.Remove(e_Structures);
            db.SaveChanges();
            //     return RedirectToAction("ShowExam", new RouteValueDictionary(
            //         new { controller = "E_Structures", action = "ShowExam", id = e_Structures.E_id })
            //     );
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
