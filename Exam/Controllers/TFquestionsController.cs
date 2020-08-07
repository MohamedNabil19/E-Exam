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

    public class TFquestionsController : Controller
    {
        private ExamEntities db = new ExamEntities();

        [CustomAuthorize("admin")]

        // GET: TFquestions
        public ActionResult Index()
        {
            var tFquestions = db.TFquestions.Include(t => t.Chapter);
            return View(tFquestions.ToList());
        }
        [CustomAuthorize("professor","admin")]
        public ActionResult Sub_Q(int id)
        {
            ViewBag.Sub_id = id;
            var sub_name = db.Subjects.Find(id);
            ViewBag.Sub_name = sub_name.name;
            var TFquestions = db.TFquestions.Where(m => m.Chapter.S_id == id);
            return View(TFquestions.ToList());
        }



        // GET: TFquestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TFquestion tFquestion = db.TFquestions.Find(id);
            if (tFquestion == null)
            {
                return HttpNotFound();
            }
            return View(tFquestion);
        }

        // GET: TFquestions/Create
        public ActionResult Create(int id)
        {
           // ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name");

            var h = db.Subjects.Single(b => b.S_id == id);
            ViewBag.Sub = h.name;
            ViewBag.CH_id = new SelectList(db.Chapters.Where(m => m.S_id == id), "CH_id", "name");
            List<SelectListItem> li_def = new List<SelectListItem>();
            li_def.Add(new SelectListItem { Text = "Select Difficulty level", Value = "0" });
            li_def.Add(new SelectListItem { Text = "A", Value = "A" });
            li_def.Add(new SelectListItem { Text = "B", Value = "B" });
            li_def.Add(new SelectListItem { Text = "C", Value = "C" });
            li_def.Add(new SelectListItem { Text = "D", Value = "D" });

            ViewBag.list_def = li_def;


            return View();
        }

        // POST: TFquestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TF_id,content,correct,def_level,CH_id")] TFquestion tFquestion)
        {
            if (ModelState.IsValid)
            {
                db.TFquestions.Add(tFquestion);
                db.SaveChanges();
                return RedirectToAction("ProfessorSubjects", "Subjects");
            }

            ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", tFquestion.CH_id);
            return View(tFquestion);
        }

        // GET: TFquestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TFquestion tFquestion = db.TFquestions.Find(id);
            if (tFquestion == null)
            {
                return HttpNotFound();
            }
         //   ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", tFquestion.CH_id);
         //   return View(tFquestion);

            ViewBag.CH_id = new SelectList(db.Chapters.Where(m => m.S_id == tFquestion.Chapter.S_id), "CH_id", "name");

            
            List<SelectListItem> li_def = new List<SelectListItem>();
            li_def.Add(new SelectListItem { Text = "Select Difficulty level", Value = "0" });
            li_def.Add(new SelectListItem { Text = "A", Value = "A" });
            li_def.Add(new SelectListItem { Text = "B", Value = "B" });
            li_def.Add(new SelectListItem { Text = "C", Value = "C" });
            li_def.Add(new SelectListItem { Text = "D", Value = "D" });

            ViewBag.list_def = li_def;



            return View(tFquestion);



        }

        // POST: TFquestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TF_id,content,correct,def_level,CH_id")] TFquestion tFquestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tFquestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ProfessorSubjects", "Subjects");
            }
            ViewBag.CH_id = new SelectList(db.Chapters, "CH_id", "name", tFquestion.CH_id);
            return View(tFquestion);
        }

        // GET: TFquestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TFquestion tFquestion = db.TFquestions.Find(id);
            if (tFquestion == null)
            {
                return HttpNotFound();
            }
            return View(tFquestion);
        }

        // POST: TFquestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TFquestion tFquestion = db.TFquestions.Find(id);
            db.TFquestions.Remove(tFquestion);
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
