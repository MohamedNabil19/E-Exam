using Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Exam.Infrastructure
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {

        private readonly string[] allowedroles;
        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            
            bool authorize = false;
            var userId = Convert.ToInt32(httpContext.Session["UserId"]);
            var username = Convert.ToString(httpContext.Session["UserName"]);
            var user_rule = (string)httpContext.Session["rule"];
            if (!string.IsNullOrEmpty(username) && userId != 0)
            {
            
                using (var context = new ExamEntities())
                {
                  //  string rule="" ;
                  //  if (user_rule == "student")
                  //  { var userRole = context.Students.Where(m => m.ST_id == userId).FirstOrDefault();
                  //      rule = userRole.rule;
                  //  }
                  //else if (user_rule == "professor")
                  //  {
                  //      var userRole = context.Professors.Where(m => m.P_id == userId).FirstOrDefault();
                  //      rule = userRole.rule;
                  //  }
                  // else if (user_rule == "admin")
                  //  { var userRole = context.Admins.Where(m => m.A_id == userId).FirstOrDefault();
                  //      rule = userRole.rule;
                  //  }




                    //       var userRole = (from u in context.ST_Rules
                    //                       join r in context.Rules on u.R_id equals r.id
                    //                       where u.ST_id == userId
                    //                       select new
                    //                       {
                    //                           r.title
                    //                       }).ToList();
                    
                    foreach (var role in allowedroles)
                    {

                        if (role == user_rule)
                        {
                            if(user_rule=="student")
                            {
                                var user = context.Students.Single(d => d.ST_id == userId);
                                if (user.approval == true)
                                { return true; }
                            }

                            else if(user_rule=="professor")
                            {
                                var user = context.Professors.Single(n => n.P_id == userId);
                                if (user.approval == true)
                                { return true; }
                            }
                            else if (user_rule=="admin")
                            {
                                return true;
                            }
                            else { return false; }
                             }
                       
                    }
                  



                }
            }





            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            filterContext.Result = new RedirectToRouteResult(
              
                new RouteValueDictionary
               {
                    { "controller", "Exam" },
                    { "action", "UnAuthorized" }
               });
        }
    }
}


