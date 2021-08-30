using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class TestScheduleController : BaseController
    {
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TestScheduleController(IHistoryTestRepository historyTestRepository,
            ICandidateRepository candidateRepository, IUnitOfWork unitOfWork)
        {
            _historyTestRepository = historyTestRepository;
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ListpageSize = new List<int>() { 5, 10, 15, 20, 50, 100 };
            return View();
        }


        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _historyTestRepository.GetData(keyword, page, pageSize);

            return Json(new
            {
                data = result.Items,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
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
                var historyTest = _historyTestRepository.Get(x => x.CandidateId == id).FirstOrDefault();

                var model = new HisToryTestInsertOrUpdateModel()
                {
                    TypeAction = 1,
                    CandidateId = historyTest.CandidateId,
                    TestEndSchedule = historyTest.TestEndSchedule,
                    TestStartSchedule = historyTest.TestStartSchedule,
                    TimeTest = historyTest.TimeTest
                };
                return View(model);
            }

            this.DropDownCandidate();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertOrUpdate(HisToryTestInsertOrUpdateModel historyTest)
        {
            //validate data input
            if (historyTest.TimeTest <= 0)
                ModelState.AddModelError("TimeTest", "Time test must be bigger 0");

            if (historyTest.TestStartSchedule <= DateTime.Now)
                ModelState.AddModelError("TestStartSchedule", "Test start schedule must be bigger Time Now");

            if (historyTest.TestEndSchedule <= historyTest.TestStartSchedule)
                ModelState.AddModelError("TestEndSchedule", "Test end schedule must be bigger Test start schedule");

            if (historyTest.TimeTest > 0 && historyTest.TestStartSchedule != null && historyTest.TestEndSchedule != null)
            {
                if (historyTest.TestEndSchedule < historyTest.TestStartSchedule.AddMinutes(historyTest.TimeTest * 3))
                    ModelState.AddModelError("TestEndSchedule", "Test end schedule invalid (TestEndSchedule >= (TestStartSchedule + (TimeTest*3)))");
            }

            if (!ModelState.IsValid)
            {
                this.DropDownCandidate(historyTest.CandidateId);
                return View(historyTest);
            }
            var result = _historyTestRepository.InsertOrUpdate(historyTest);

            if (result == true)
            {
                if (historyTest.TypeAction == 0)
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
                if (historyTest.TypeAction == 0)
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
            var result = _historyTestRepository.Locked(id);

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

        private void DropDownCandidate(int candidateId = 0)
        {
            int selectDrop = 0;
            var lstCandi = _candidateRepository.Get(filter: x => x.Status == false
                           && x.Deleted == false, orderBy: c => c.OrderByDescending(y => y.Id)).ToList();

            if (candidateId != 0)
            {
                foreach (var item in lstCandi)
                {
                    if (item.Id == candidateId)
                        selectDrop = lstCandi.IndexOf(item);
                }
            }
            ViewBag.CandidateId = new SelectList(lstCandi, "Id", "Name", selectDrop);
        }
    }
}
