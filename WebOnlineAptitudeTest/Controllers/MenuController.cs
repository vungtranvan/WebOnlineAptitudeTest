using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;


namespace WebOnlineAptitudeTest.Controllers
{
    public class MenuController : BaseFrController
    {
        // GET: Menu
        public ActionResult Menu()
        {
            var xml = XDocument.Load(Server.MapPath("~/etc/menu.xml"));
            var menus = xml.Descendants("menus").FirstOrDefault();
            var menu = menus.Descendants("menu").ToList();

            ViewBag.menu = menu;

            return View();
        }

        // GET: MenuMobile
        public ActionResult MenuMobile()
        {
            return View();
        }
    }
}