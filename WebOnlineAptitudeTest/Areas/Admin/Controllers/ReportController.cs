using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        private readonly ITestScheduleRepository _testScheduleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReportController(ITestScheduleRepository testScheduleRepository, IUnitOfWork unitOfWork)
        {
            _testScheduleRepository = testScheduleRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ListpageSize = new List<int>() { 10, 15, 20, 50, 100 };
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(DateTime? fromDate, DateTime? toDate, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;

            if (fromDate == null)
                fromDate = new DateTime(2000, 01, 01);

            if (toDate == null)
                toDate = DateTime.Now;

            var result = _testScheduleRepository.CountCandidate(fromDate, toDate, page, pageSize);

            var resultData = JsonConvert.SerializeObject(result.Items, Formatting.Indented,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });

            var json = Json(new
            {
                data = resultData,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }
    }
}