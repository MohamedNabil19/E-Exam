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
    [CustomAuthorize("professor", "admin")]

    public class MCQquestionsController : Controller
    {
        private ExamEntities db = new ExamEntities();

        [CustomAuthorize( "admin")]

        // GET: MCQquestions
        public ActionResult Index()
        {
            var mCQquestions = db.MCQquestions.Include(m => m.Chapter);
            return View(mCQquestions.ToList());
        }


        public ActionResult Sub_Q(int id)
        {
            ViewBag.Sub_id = id;
            var sub_name = db.Subjects.Find(id);
            ViewBag.Sub_name = sub_name.name;
            var mCQquestions = db.MCQquestions.Where(m=>m.Chapter.S_id==id);
            return View(mCQquestions.ToList());
        }

        // GET: MCQquestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MCQquestion mCQquestion = db.MCQquestions.Find(id);
            if (mCQquestion == null)
            {
                return HttpNotFound();
            }

          // var h = db.Subjects.Single(b => b.S_id == id);
          // ViewBag.Sub = h.name;

            return View(mCQquestion);
        }
        // GET: MCQquestions/Create
        public ActionResult Create(int id)
        {
             var h = db.Subjects.Single(b => b.S_id == id);
            ViewBag.Sub = h.name;
            ViewBag.CH_id = new SelectList(db.Chapters.Where(m=>m.S_id==id), "CH_id", "name");
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Correct Answer", Value = "0" });
            li.Add(new SelectListItem { Text = "option1", Value = "option1" });
            li.Add(new SelectListItem { Text = "option2", Value = "option2" });
            li.Add(new SelectListItem { Text = "option3", Value = "option3" });
            li.Add(new SelectListItem { Text = "option4", Value = "option4" });
            
            ViewData["options"] = li;
           List<SelectListItem> li_def = new List<SelectListItem>();
           li_def.Add(new SelectListItem { Text = "Select Difficulty level", Value = "0" });
           li_def.Add(new SelectListItem { Text = "A", Value = "A" });
           li_def.Add(new SelectListItem { Text = "B", Value = "B" });
           li_def.Add(new SelectListItem { Text = "C", Value = "C" });
           li_def.Add(new SelectListItem { Text = "D", Value = "D" });
        
           ViewBag.list_def = li_def;

            return View();


        }

        // POST: MCQquestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MQ_id,content,option1,optoin2,option3,option4,correct,def_level,CH_id")] MCQquestion mCQquestion)
        {
            if (ModelState.IsValid)
            {
                db.MCQquestions.Add(mCQquestion);
                db.SaveChanges();
                return RedirectToAction("ProfessorSubjects", "Subjects");
            }

            ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", mCQquestion.CH_id);
            return View(mCQquestion);
        }


        // GET: MCQquestions/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MCQquestion mCQquestion = db.MCQquestions.Find(id);
            if (mCQquestion == null)
            {
                return HttpNotFound();
            }
            //  ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", mCQquestion.CH_id);


            // var h = db.Subjects.Single(b => b.S_id == id);
            // ViewBag.Sub = h.name;
            //   ViewBag.CH_id = new SelectList(db.Chapters.Where(m => m.S_id == id), "CH_id", "name");
            ViewBag.CH_id = new SelectList(db.Chapters.Where(m => m.S_id == mCQquestion.Chapter.S_id), "CH_id", "name");


            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Correct Answer", Value = "0" });
            li.Add(new SelectListItem { Text = "option1", Value = "option1" });
            li.Add(new SelectListItem { Text = "option2", Value = "option2" });
            li.Add(new SelectListItem { Text = "option3", Value = "option3" });
            li.Add(new SelectListItem { Text = "option4", Value = "option4" });

            ViewData["options"] = li;
            List<SelectListItem> li_def = new List<SelectListItem>();
            li_def.Add(new SelectListItem { Text = "Select Difficulty level", Value = "0" });
            li_def.Add(new SelectListItem { Text = "A", Value = "A" });
            li_def.Add(new SelectListItem { Text = "B", Value = "B" });
            li_def.Add(new SelectListItem { Text = "C", Value = "C" });
            li_def.Add(new SelectListItem { Text = "D", Value = "D" });

            ViewBag.list_def = li_def;


            return View(mCQquestion);
        }

        // POST: MCQquestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MQ_id,content,option1,optoin2,option3,option4,correct,def_level,CH_id")] MCQquestion mCQquestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mCQquestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProfessorSubjects", "Subjects");
            }
            ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", mCQquestion.CH_id);
            return View(mCQquestion);
        }

        // GET: MCQquestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MCQquestion mCQquestion = db.MCQquestions.Find(id);
            if (mCQquestion == null)
            {
                return HttpNotFound();
            }
            return View(mCQquestion);
        }

        // POST: MCQquestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MCQquestion mCQquestion = db.MCQquestions.Find(id);
            db.MCQquestions.Remove(mCQquestion);
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
