using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class HistoryTestsController : BaseController
    {
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly ITestScheduleRepository _testScheduleRepository;
        private readonly IUnitOfWork _unitOfWork;
        public HistoryTestsController(IHistoryTestRepository historyTestRepository,
            ITestScheduleRepository testScheduleRepository,
            IUnitOfWork unitOfWork)
        {
            _historyTestRepository = historyTestRepository;
            _testScheduleRepository = testScheduleRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? id)
        {
            _historyTestRepository.UpdateStatusCandidateAndHistoryTest();
            ViewBag.ListpageSize = new List<int>() { 10, 15, 20, 50, 100 };

            if (id != null)
            {
                this.DropDownTestSchedule(id.Value);
                return View();
            }

            this.DropDownTestSchedule();
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int idTeschedule, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _historyTestRepository.GetData(keyword, idTeschedule, page, pageSize);

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

        [HttpPost]
        public JsonResult Locked(int id)
        {
            var title = "Notification";
            var cadi = _historyTestRepository.Locked(id);

            if (!cadi)
            {
                return Json(new
                {
                    message = "Delete Error !!!",
                    status = false,
                    title
                });
            }

            return Json(new
            {
                message = "Delete Successfull !!!",
                status = true,
                title
            });
        }

        [HttpGet]
        public JsonResult GetCategoryExamName(int id)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var cadi = _unitOfWork.DbContext.CategoryExams.Where(x => x.Id.Equals(id)).FirstOrDefault();

            return Json(new
            {
                name = cadi.Name
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTestSheduleName(int id)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var testS = _unitOfWork.DbContext.TestSchedules.Where(x => x.Id.Equals(id)).FirstOrDefault();
            return Json(new
            {
                name = testS.Name
            }, JsonRequestBehavior.AllowGet);
        }


        private void DropDownTestSchedule(int id = 0)
        {
            var lstCateEx = _testScheduleRepository.Get(orderBy: x => x.OrderByDescending(y => y.Id)).ToList();
            ViewBag.NewsItemList = new SelectList(lstCateEx, "Id", "Name", id);
        }
    }
}