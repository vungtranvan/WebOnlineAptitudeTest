using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class MenusController : Controller
    {
        // GET: Admin/Menu
        public ActionResult Index()
        {
            ViewBag.section = CreateMenu();
            return View();
        }
        protected List<XElement> CreateMenu()
        {
            var xml = XDocument.Load(Server.MapPath("~/Areas/Admin/etc/menu.xml"));
            var menus = xml.Descendants("menus").FirstOrDefault();
            var section = menus.Descendants("section").ToList();
            return section;
        }
    }
}