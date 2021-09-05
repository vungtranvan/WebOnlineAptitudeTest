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
                    "Admin_HistoryTests",
                    "admin/HistoryTests/{id}",
                    defaults: new { controller = "HistoryTests", action = "Index", id = UrlParameter.Optional }
                );

            context.MapRoute(
                "Admin_default",
                "admin/{controller}/{action}/{id}",
                defaults: new { controller = "Auth", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}