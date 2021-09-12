using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.TestSchedules;
using WebOnlineAptitudeTest.Enums;
using WebOnlineAptitudeTest.Helper;
using WebOnlineAptitudeTest.Models;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    [BackEndAuthorize]
    public class TestScheduleController : Controller
    {
        private readonly ITestScheduleRepository _testScheduleRepository;
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TestScheduleController(ITestScheduleRepository testScheduleRepository, IHistoryTestRepository historyTestRepository,
            ICandidateRepository candidateRepository, IUnitOfWork unitOfWork)
        {
            _testScheduleRepository = testScheduleRepository;
            _historyTestRepository = historyTestRepository;
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            _historyTestRepository.UpdateStatusCandidateAndHistoryTest();
            ViewBag.ListpageSize = new List<int>() { 5, 10, 15, 20, 50, 100 };
            return View();
        }


        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _testScheduleRepository.GetData(keyword, page, pageSize);

            var json = Json(new
            {
                data = result.Items,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult InsertOrUpdate(int? id)
        {
            if (id != null)
            {
                if (_testScheduleRepository.CheckStatus(id.Value))
                {
                    var model = _testScheduleRepository.GetInsertOrUpdateRequest(id.Value);
                    this.MultiSelectListCandidate(model.CandidateId);
                    return View(model);
                }
                TempData["XMessage"] = new XMessage("Notification", "Unable to update because the exam is starting or has ended !!!", EnumCategoryMess.error);
                return RedirectToAction("Index");
            }

            this.MultiSelectListCandidate();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertOrUpdate(TestScheduleInsertOrUpdateRequest model)
        {
            //validate data input
            if (model.DateStart <= DateTime.Now)
                ModelState.AddModelError("DateStart", "Test start schedule must be bigger Time Now");

            if (model.DateStart != null && model.DateEnd != null)
            {
                var countTime = _unitOfWork.DbContext.CategoryExams.Select(x => x.TimeTest).Sum();
                var dateTimeMinValid = model.DateStart.AddMinutes(countTime);
                if (model.DateEnd < dateTimeMinValid)
                    ModelState.AddModelError("DateEnd", $"Test end schedule invalid (DateEnd >= {dateTimeMinValid.ToString("MM/dd/yyyy HH:mm")})");
            }

            if (!ModelState.IsValid)
            {
                this.MultiSelectListCandidate(model.CandidateId);
                return View(model);
            }
            var result = _testScheduleRepository.InsertOrUpdate(model);

            //Send Email 
            foreach (var item in model.CandidateId)
            {
                var candi = _candidateRepository.GetSingleById(item);
                string content = System.IO.File.ReadAllText(Server.MapPath("/Content/assets/backend/template/sendMailTestSchedule_template.html"));
                content = content.Replace("{{StartDate}}", model.DateStart.ToString("MM/dd/yyyy HH:mm"));
                content = content.Replace("{{EndDate}}", model.DateEnd.ToString("MM/dd/yyyy HH:mm"));
                content = content.Replace("{{UserName}}", candi.UserName);
                content = content.Replace("{{Password}}", candi.Password);
                var adminEmail = ConfigHelper.GetByKey("AdminEmail");
                MailHelper.SendMail(candi.Email, "Welcome mail and Online Aptitude Test", content);
            }

            if (result == true)
            {
                if (model.Id == 0)
                {
                    TempData["XMessage"] = new XMessage("Notification", "Add Successfull !!!", EnumCategoryMess.success);
                }
                else
                {
                    TempData["XMessage"] = new XMessage("Notification", "Edit Successfull !!!", EnumCategoryMess.success);
                }
            }
            else
            {
                if (model.Id == 0)
                {
                    TempData["XMessage"] = new XMessage("Notification", "Add Error !!!", EnumCategoryMess.error);
                }
                else
                {
                    TempData["XMessage"] = new XMessage("Notification", "Edit Error !!!", EnumCategoryMess.error);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Locked(int id)
        {
            var title = "Notification";
            var result = _testScheduleRepository.Locked(id);

            if (!result)
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

        private void MultiSelectListCandidate(List<int> lstcandidateId = null)
        {
            var lstCandi = _candidateRepository.Get(filter: x => (x.Status == EnumStatusCandidate.Undone || x.Status == EnumStatusCandidate.New)
                           && x.Deleted == false, orderBy: c => c.OrderByDescending(y => y.Id)).ToList();

            if (lstcandidateId != null)
            {
                foreach (var item in _candidateRepository.Get(filter: x => x.Status == EnumStatusCandidate.Scheduled))
                {
                    foreach (var c in lstcandidateId)
                    {
                        if (item.Id == c)
                        {
                            lstCandi.Add(item);
                        }
                    }
                }

                ViewBag.NewsItemList = new MultiSelectList(lstCandi, "Id", "Name", lstcandidateId);
            }
            else
            {
                ViewBag.NewsItemList = new MultiSelectList(lstCandi, "Id", "Name");
            }
        }
    }
}
