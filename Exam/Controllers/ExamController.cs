using System;
using System.Data.Entity;
using Exam.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using Exam.Infrastructure;
using System.Text;
using System.Security.Cryptography;


namespace Exam.Controllers
{
    
    
        public static class Encryptor
        {
            public static string MD5Hash(string text)
            {
                MD5 md5 = new MD5CryptoServiceProvider();

                //compute hash from the bytes of text  
                md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

                //get hash result after compute it  
                byte[] result = md5.Hash;

                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    //change it into 2 hexadecimal digits  
                    //for each byte  
                    strBuilder.Append(result[i].ToString("x2"));
                }

                return strBuilder.ToString();
            }
        }
   
    public class ExamController : Controller
    {

        private ExamEntities db = new ExamEntities();
        static List<MCQquestion> LMCQq = new List<MCQquestion>();

        static List<TFquestion> LTFq = new List<TFquestion>();
        static List<E_Structures> L_E_st_MCQ = new List<E_Structures>();
        static List<E_Structures> L_E_st_TF = new List<E_Structures>();


        private Queue<E_Structures> listofst = new Queue<E_Structures>();

        private Queue<AnswerExam> listofq = new Queue<AnswerExam>();

        [CustomAuthenticationFilter]
        // GET: Exam
        [CustomAuthorize("student")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult example()
        {
            return View("example");
        }


        [HttpGet]
        public ActionResult LoginStudent()
        {
            return View("LoginStudent");
        }


        [HttpPost]
        public ActionResult LoginStudent(Student model)
        {
            if (ModelState.IsValid)
            {
                string oldpassword = model.passwod;
                model.passwod = Encryptor.MD5Hash(oldpassword);

                using (var context = new ExamEntities())
                {
                    Student user = context.Students
                                       .Where(u => u.email == model.email && u.passwod == model.passwod)
                                       .FirstOrDefault();

                    if (user != null)
                    {
                        Session["rule"] = user.rule;
                        Session["UserName"] = user.name;
                        Session["UserId"] = user.ST_id;
                        return RedirectToAction("StudentPage", "Students");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid User Name or Password");
                        return View(model);
                    }
                }
            }
            else
            {
                return View(model);
            }
        }


        [HttpGet]
        public ActionResult LoginAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginAdmin(Admin model)
        {
            if (ModelState.IsValid)
            {
                string oldpassword = model.password;
                model.password = Encryptor.MD5Hash(oldpassword);

                using (var context = new ExamEntities())
                {
                    Admin user = context.Admins
                                       .Where(u => u.email == model.email && u.password == model.password)
                                       .FirstOrDefault();

                    if (user != null)
                    {
                        Session["rule"] = user.rule;
                        Session["UserName"] = user.name;
                        Session["UserId"] = user.A_id;
                        //Add Admin Page
                        return RedirectToAction("AdminPage", "Admins");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid User Name or Password");
                        return View(model);
                    }
                }
            }
            else
            {
                return View(model);
            }
        }










        [HttpGet]
        public ActionResult LoginProfessor()
        {
            return View("");
        }


        [HttpPost]
        public ActionResult LoginProfessor(Professor model)
        {
            if (ModelState.IsValid)
            {
                string oldpassword = model.password;
                model.password = Encryptor.MD5Hash(oldpassword);
                using (var context = new ExamEntities())
                {
                    Professor user = context.Professors
                                       .Where(u => u.emai == model.emai && u.password == model.password)
                                       .FirstOrDefault();

                    if (user != null)
                    {
                        Session["rule"] = user.rule;
                        Session["UserName"] = user.name;
                        Session["UserId"] = user.P_id;
                        return RedirectToAction("ProfessorPage", "Professors");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid User Name or Password");
                        return View(model);
                    }
                }
            }
            else
            {
                return View(model);
            }
        }




        // GET: Admins/Create
        public ActionResult RegisterAdmin()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterAdmin([Bind(Include = "A_id,name,mobile,N_N,A_image,email,password,A_image,user_image_data")] Admin admin)
        {
            if (admin.user_image_data != null)
            {
                admin.A_image = "~/Photos/" + admin.user_image_data.FileName;
                string path = Server.MapPath("~/Photos/");
                //store image in folder
                admin.user_image_data.SaveAs(path + admin.user_image_data.FileName);
            }

            if (ModelState.IsValid)
            {
                string oldpassword = admin.password;
                admin.password = Encryptor.MD5Hash(oldpassword);

                admin.rule = "admin";
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("LoginAdmin", "Exam");
            }

            return View(admin);
        }



        [HttpGet]
        public ActionResult RegisterStudent()
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

        public ActionResult RegisterStudent([Bind(Include = "ST_id,name,email,passwod,mobile,N_N,rule,approval,L_id,Dep_id,total,ST_image,user_image_data")] Student student)
        {
            student.ST_image = "~/Photos/" + student.user_image_data.FileName;
            string path = Server.MapPath("~/Photos/");
            //store image in folder
            student.user_image_data.SaveAs(path + student.user_image_data.FileName);
            //  student.user_image_data.SaveAs(Server.MapPath("Photos") + "/" + student.user_image_data.FileName);
            student.approval = false;
            student.rule = "student";
            student.total = 0.0;


            if (ModelState.IsValid)
            {
                string oldpassword = student.passwod;
               student.passwod= Encryptor.MD5Hash(oldpassword);
               db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("StudentPage", "Students");
            }

            ViewBag.Dep_id = new SelectList(db.Departments, "Dep_id", "name", student.Dep_id);
            ViewBag.L_id = new SelectList(db.Levels, "L_id", "L_id", student.L_id);
            return View(student);
        }


        // GET: Professors/Create
        public ActionResult RegisterProfessor()
        {
            return View();
        }

        // POST: Professors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterProfessor([Bind(Include = "P_id,name,emai,password,mobile,N_N,rule,approval,P_image,user_image_data")] Professor professor)
        {
            professor.P_image = "~/Photos/" + professor.user_image_data.FileName;
            string path = Server.MapPath("~/Photos/");
            //store image in folder
            professor.user_image_data.SaveAs(path + professor.user_image_data.FileName);
            professor.approval = false;
            professor.rule = "professor";
          
            if (ModelState.IsValid)
            {
                string oldpassword = professor.password;
                professor.password = Encryptor.MD5Hash(oldpassword);

                db.Professors.Add(professor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(professor);
        }


       


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["UserName"] = string.Empty;
            Session["UserId"] = string.Empty;
            return RedirectToAction("Login", "Exam");
        }





        public ActionResult UnAuthorized()
        {
            return View();
        }


        // Exam Solutions



//       public ActionResult StructureExam(int id)
//       {
//           var Structure = db.E_Structures.Where(m => m.E_id == id).ToList();
//
//           foreach (var item in Structure)
//           {
//               listofst.Enqueue(item);
//           }
//
//           return View();
//       }
//
//       [CustomAuthorize("student")]
//       public ActionResult NextQuestion(int id)
//       {
//           var user_id = (int)Session["UserId"];
//           List<E_Structures> list_st_exam = db.E_Structures.Where(m => m.E_id == id && m.post == false).ToList();
//           List<SelectListItem> li = new List<SelectListItem>();
//           li.Add(new SelectListItem { Text = "Select Correct Answer", Value = "0" });
//           li.Add(new SelectListItem { Text = "option1", Value = "option1" });
//           li.Add(new SelectListItem { Text = "option2", Value = "option2" });
//           li.Add(new SelectListItem { Text = "option3", Value = "option3" });
//           li.Add(new SelectListItem { Text = "option4", Value = "option4" });
//
//           ViewData["options"] = li;
//
//
//           List<SelectListItem> liTF = new List<SelectListItem>();
//           liTF.Add(new SelectListItem { Text = "Select Correct Answer", Value = "" });
//           liTF.Add(new SelectListItem { Text = "True", Value = "True" });
//           liTF.Add(new SelectListItem { Text = "false", Value = "False" });
//
//           ViewData["opTF"] = liTF;
//
//           if (list_st_exam.Count > 0)
//           {
//               var st_exam = list_st_exam.First();
//
//               var student = db.Results.SingleOrDefault(m => m.ST_id == user_id && m.S_id == st_exam.Exam.S_id);
//
//               if (student.StartExam == true)
//               {
//                   st_exam.post = true;
//                   db.SaveChanges();
//                   if (st_exam.Type_Q == false)
//                   {
//                       var qs = db.MCQquestions.Where(m => m.CH_id == st_exam.CH_id && m.def_level == st_exam.def_level).ToList();
//                       var MCQq = qs.OrderBy(o => Guid.NewGuid()).Take(st_exam.NumofQ);
//                       // add view with iEnumable <McQ>
//                       return View("McqQ", MCQq);
//
//                   }
//
//                   else
//                   {
//                       var qs = db.TFquestions.Where(m => m.CH_id == st_exam.CH_id && m.def_level == st_exam.def_level).ToList();
//                       var TFq = qs.OrderBy(o => Guid.NewGuid()).Take(st_exam.NumofQ);
//                       // add view with model iEnumable <TF>
//
//                       return View("TFQ", TFq);
//                   }
//
//               }
//               else
//               {
//                   return View("ClosingExam");
//               }
//           }
//
//           else
//           {
//               var repostq = db.E_Structures.Where(m => m.E_id == id && m.post == true).ToList();
//
//               foreach (var item in repostq)
//               {
//                   item.post = false;
//
//               }
//
//               db.SaveChanges();
//               int g = (int)repostq.First().Exam.S_id;
//               var student = db.Results.SingleOrDefault(m => m.ST_id == user_id && m.S_id == g);
//               student.StartExam = false;
//               db.SaveChanges();
//
//
//
//               //add view if the student the exam ()
//               return View("NextQuestionfinish");
//           }
//       }
//       [CustomAuthenticationFilter]
//
//       public ActionResult CorrectAnswerMCQ()
//       {
//           var qq = db.MCQquestions.First();
//           List<SelectListItem> li = new List<SelectListItem>();
//           li.Add(new SelectListItem { Text = "Select Correct Answer", Value = "0" });
//           li.Add(new SelectListItem { Text = "option1", Value = "option1" });
//           li.Add(new SelectListItem { Text = "option2", Value = "option2" });
//           li.Add(new SelectListItem { Text = "option3", Value = "option3" });
//           li.Add(new SelectListItem { Text = "option4", Value = "option4" });
//
//           ViewData["options"] = li;
//
//           return View("UnAuthorized", qq);
//       }
//
//
//       [HttpPost]
//       public ActionResult CorrectAnswer(Questions q)
//       {
//           if (q.MCQ.MQ_id > 0)
//           {
//               var user_id = (int)Session["UserId"];
//               var qq = db.MCQquestions.Find(q.MCQ.MQ_id);
//               var st_sub = db.Results.Single(m => m.ST_id == user_id && m.S_id == qq.Chapter.S_id);
//               double r = st_sub.result1;
//               if (q.MCQ.correct == q.MCQ.Answer)
//               {
//                   st_sub.result1 = r + 5;
//                   db.SaveChanges();
//
//               }
//
//               return RedirectToAction("questions", "Exam");
//           }
//           else if(q.TF.TF_id > 0)
//           {
//
//               var user_id = (int)Session["UserId"];
//               var qq = db.MCQquestions.Find(q.TF.TF_id);
//               var st_sub = db.Results.Single(m => m.ST_id == user_id && m.S_id == qq.Chapter.S_id);
//               double r = st_sub.result1;
//               if (q.TF.correct == q.TF.Answer)
//               {
//                   st_sub.result1 = r + 5;
//                   db.SaveChanges();
//
//               }
//
//               return RedirectToAction("questions", "Exam");
//           }
//           
//           else
//           {
//               return View("ClosingExam");
//           }
//           
//           //  }
//           //  //add partialView if the solution submit
//           //  return PartialView("kbhb");
//           //
//       }
//

        
        
        
        
        
                                              



        [HttpPost]
        public ActionResult CorrectAnswerMCQ(MCQquestion q)
        {

          //  if (Request.IsAjaxRequest())
           // {
                var user_id = (int)Session["UserId"];
                var qq = db.MCQquestions.Find(q.MQ_id);
                var st_sub = db.Results.Single(m => m.ST_id == user_id && m.S_id == qq.Chapter.S_id);
                double r = st_sub.result1;
                if (q.correct == q.Answer)
                {
                    st_sub.result1 = r + 5;
                    db.SaveChanges();

                }

                return RedirectToAction("questions","Exam");

          //  }
          //  //add partialView if the solution submit
          //  return PartialView("kbhb");
          //
        }

        public ActionResult CorrectAnswerTF(TFquestion q)
        {
         //   if (Request.IsAjaxRequest())
           // {
                var user_id = (int)Session["UserId"];
                var qq = db.TFquestions.Find(q.TF_id);
                var st_sub = db.Results.Single(m => m.ST_id == user_id && m.S_id == qq.Chapter.S_id);
                double r = st_sub.result1;
                if (q.correct == q.Answer)
                {
                    st_sub.result1 = r + 5;
                    db.SaveChanges();

                }

              //  return PartialView("DoneSol");
                return RedirectToAction("questions", "Exam");

            
        //    //add partialView if the solution submit
        //    return PartialView("kbhb");

        }


        public ActionResult StudentExams()
        {
            var user_id = (int)Session["UserId"];
            var subjects= db.Results.Include(r => r.Subject).Where(b=>b.ST_id==user_id).ToList();

            return View(subjects);
        }

        public ActionResult ModelExam(int id)
        {
            var Models = db.ExamQuestions.Where(n => n.StausPost == true && n.S_id == id);
            
            return View(Models);
        }

        // [CustomAuthenticationFilter]
        [CustomAuthorize("student")]
        public ActionResult PreExam(int id)
        {
            L_E_st_MCQ.Clear();
            L_E_st_TF.Clear();
            LMCQq.Clear();
            LTFq.Clear();
            var user_id = (int)Session["UserId"];
            var listofstructureMCQ = db.E_Structures.Where(m => m.E_id == id && m.Type_Q==false).ToList();
            var listofstructureTF = db.E_Structures.Where(m => m.E_id == id && m.Type_Q == true).ToList();
          //  int dd = listofstructureMCQ.First().Chapter.S_id;
            var exam = db.ExamQuestions.Single(v=>v.E_id==id);
            int ds = (int)exam.S_id;
            //  var student_status = db.Results.Single(m => m.ST_id == user_id && m.S_id == dd);
              var student_status = db.Results.Single(m => m.ST_id == user_id && m.S_id == ds);
            L_E_st_MCQ.AddRange(listofstructureMCQ);
              L_E_st_TF.AddRange(listofstructureTF);
            
          if (student_status.StartExam == true)
          {
                foreach (var item in listofstructureMCQ)
                {
                    var qs = db.MCQquestions.Where(m => m.CH_id == item.CH_id && m.def_level == item.def_level).ToList();
                    var MCQq = qs.OrderBy(o => Guid.NewGuid()).Take(item.NumofQ);
                    LMCQq.AddRange(MCQq);
                }
                foreach (var item in listofstructureTF)
                {
                    var qs = db.TFquestions.Where(m => m.CH_id == item.CH_id && m.def_level == item.def_level).ToList();
                    var TFq = qs.OrderBy(o => Guid.NewGuid()).Take(item.NumofQ);
                    LTFq.AddRange(TFq);
                }

                student_status.StartExam = false;
                db.SaveChanges();
                return View();

            }

            else
            { 
                 return View("ClosingExam");
            }


        }

        [CustomAuthorize("student")]

        [HttpGet]
        public PartialViewResult questions()
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "Select Correct Answer", Value = "0" });
            li.Add(new SelectListItem { Text = "option1", Value = "option1" });
            li.Add(new SelectListItem { Text = "option2", Value = "option2" });
            li.Add(new SelectListItem { Text = "option3", Value = "option3" });
            li.Add(new SelectListItem { Text = "option4", Value = "option4" });

            ViewData["options"] = li;


            List<SelectListItem> liTF = new List<SelectListItem>();
            liTF.Add(new SelectListItem { Text = "Select Correct Answer", Value = "" });
            liTF.Add(new SelectListItem { Text = "True", Value = "True" });
            liTF.Add(new SelectListItem { Text = "false", Value = "False" });

            ViewData["opTF"] = liTF;
           
            if (LMCQq.Count > 0)
                {
                MCQquestion question = new MCQquestion();
                question = LMCQq.First();
                    LMCQq.Remove(question);
                    return PartialView("CorrectAnswer", question);
                }

                else if (LTFq.Count > 0)
                {
                    TFquestion question = new TFquestion();
                    question = LTFq.First();
                    LTFq.Remove(question);
                    return PartialView("CorrectAnswerTF", question);
                }
                else
                {
                    return PartialView("DoneSol");
                }
            
        }
        public ActionResult Login()
        {
            return View();
        }

//
//      [HttpPost]
//      public ActionResult MCQquestions(MCQquestion q)
//      {
//
//          if (Request.IsAjaxRequest())
//          {
//              var user_id = (int)Session["UserId"];
//              var qq = db.MCQquestions.Find(q.MQ_id);
//              var st_sub = db.Results.Single(m => m.ST_id == user_id && m.S_id == qq.Chapter.S_id);
//              double r = st_sub.result1;
//              if (q.correct == q.Answer)
//              {
//                  st_sub.result1 = r + 5;
//                  db.SaveChanges();
//
//              }
//
//              return RedirectToAction("questions","Exam");
//
//          }
//          //add partialView if the solution submit
//          return PartialView("kbhb");
//
//      }
//
//      [HttpPost]
//      public ActionResult TFquestions(TFquestion q)
//      {
//
//          if (Request.IsAjaxRequest())
//          {
//              var user_id = (int)Session["UserId"];
//              var qq = db.TFquestions.Find(q.TF_id);
//              var st_sub = db.Results.Single(m => m.ST_id == user_id && m.S_id == qq.Chapter.S_id);
//              double r = st_sub.result1;
//              if (q.correct == q.Answer)
//              {
//                  st_sub.result1 = r + 5;
//                  db.SaveChanges();
//
//              }
//
//              return RedirectToAction("questions", "Exam");
//
//          }
//          //add partialView if the solution submit
//          return PartialView("kbhb");
//
//      }
//


    }





}
