using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOnlineAptitudeTest.Models
{
    public class FrontEndAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["CandidateTest"].Equals("") || httpContext.Session["CandidateTest"] == null)
            {
                return false;
            }
            return true;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["CandidateTest"].Equals("") || filterContext.HttpContext.Session["CandidateTest"] == null)
            {
                filterContext.Result = new RedirectResult("~/logins");
            }
        }
    }
}