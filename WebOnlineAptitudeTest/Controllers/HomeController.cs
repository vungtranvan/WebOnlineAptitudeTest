using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Controllers
{
    public class HomeController : BaseFrController
    {
        public HomeController()
        {
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AptidudeTest(FormCollection testRequest)
        {

            string reQuestQuest = "";

            var collection  =  testRequest.Get("collection");

            foreach (var key in testRequest.AllKeys)
            {
                if (key.Contains("collection"))
                {
                    reQuestQuest = testRequest.Get(key);
                }
            }
            var abc = HttpUtility.ParseQueryString(collection);


            var value = abc.Get("q2");
;

            List<string> lstAppendColumn = new List<string>();
            lstAppendColumn.Add("First");
            lstAppendColumn.Add("Second");
            lstAppendColumn.Add("Third");

            string Response = JsonConvert.SerializeObject(lstAppendColumn);


            return Json(new { Response, Status = "Success", PartName = "123" }, JsonRequestBehavior.AllowGet);
        }
    }
}