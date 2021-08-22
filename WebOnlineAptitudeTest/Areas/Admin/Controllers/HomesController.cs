using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class HomesController : BaseController
    {
        // GET: Admin/Homes
        public ActionResult Index()
        {
            var xml = XDocument.Load(Server.MapPath("~/Areas/Admin/etc/menu.xml"));
            var menus = xml.Descendants("menus").FirstOrDefault();
            var section = menus.Descendants("section").ToList();

            ViewBag.section = section;

            return View();
        }

        // GET: Admin/Menu
    
    }
}