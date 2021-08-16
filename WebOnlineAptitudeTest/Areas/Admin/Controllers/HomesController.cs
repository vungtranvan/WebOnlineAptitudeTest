using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class HomesController : Controller
    {
        // GET: Admin/Homes
        public ActionResult Index()
        {
            return View();
        }
    }
}