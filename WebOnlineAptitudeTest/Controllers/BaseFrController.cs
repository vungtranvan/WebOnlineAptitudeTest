using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOnlineAptitudeTest.Controllers
{
    public class BaseFrController : Controller
    {
        public BaseFrController()
        {
            if (System.Web.HttpContext.Current.Session["CandidateTest"].Equals(""))
            {
                System.Web.HttpContext.Current.Response.Redirect("~/logins");
            }
        }
    }
}