using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOnlineAptitudeTest.Models
{
    public class BackEndAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserAdmin"].Equals("") || httpContext.Session["UserAdmin"] == null)
            {
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserAdmin"].Equals("") || filterContext.HttpContext.Session["UserAdmin"] == null)
            {
                filterContext.Result = new RedirectResult("~/admin/login");
            }
        }
    }
}