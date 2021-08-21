using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebOnlineAptitudeTest
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LoginFr",
                url: "logins",
                defaults: new { controller = "AuthFrontend", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LogoutFr",
                url: "logout",
                defaults: new { controller = "AuthFrontend", action = "Logout", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
