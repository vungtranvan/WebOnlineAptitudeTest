using System.Web.Mvc;

namespace WebOnlineAptitudeTest.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "Admin_login",
               "admin/login",
               defaults: new { controller = "Auth", action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
               "Admin_logout",
               "admin/logout",
               defaults: new { controller = "Auth", action = "Logout", id = UrlParameter.Optional }
           );
            context.MapRoute(
              "CandidateAdmin_home",
              "admin/candidate",
              defaults: new { controller = "Candidate", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
              "CandidateAdmin_AddorEdit",
              "admin/candidate/addoredit/{id}",
              defaults: new { controller = "Candidate", action = "InsertOrUpdate", id = UrlParameter.Optional }
          );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Homes", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}