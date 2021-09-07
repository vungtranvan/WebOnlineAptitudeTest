using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.User;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Controllers
{
    public class AuthFrontendController : Controller
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly ITestScheduleRepository _testScheduleRepository;

        public AuthFrontendController(ICandidateRepository candidateRepository, ITestScheduleRepository testScheduleRepository)
        {
            _candidateRepository = candidateRepository;
            _testScheduleRepository = testScheduleRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            LoginRequest model = new LoginRequest();

            if (!Session["CandidateTest"].Equals(""))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!Session["RememberLoginCandidate"].Equals(""))
            {
                model = JsonConvert.DeserializeObject<LoginRequest>(Session["RememberLoginCandidate"].ToString());
            }

            ViewBag.Error = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginRequest request, string rememberMe)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var candi = _candidateRepository.Get(filter: x => x.UserName.Equals(request.UserName) && x.Deleted == false).FirstOrDefault();

            if (candi == null)
            {
                ViewBag.Error = "UserName does not exist!!!";
                return View(request);
            }

            if (!candi.Password.Equals(request.Password))
            {
                ViewBag.Error = "Incorrect password!!!";
                return View(request);
            }

            //var testSchedule = _testScheduleRepository.GetMulti(x => x.Deleted == false && x.HistoryTests.Where(y => y.CandidateId.Equals(candi.Id)).Count() > 0, new[] { "HistoryTests" }).FirstOrDefault();

            //if (testSchedule == null)
            //{
            //    ViewBag.Error = "You don't have an exam schedule yet!";
            //    return View(request);
            //}

            //if (testSchedule.DateStart > DateTime.Now)
            //{
            //    ViewBag.Error = "It's not time for the exam yet!";
            //    return View(request);
            //}

            //if (testSchedule.DateEnd < DateTime.Now)
            //{
            //    ViewBag.Error = "The exam is over!";
            //    return View(request);
            //}

            Session["CandidateTest"] = candi.Id;
            Session["DisplayNameCandidate"] = candi.Name;
            Session["ImageCandidate"] = candi.Image;

            if (rememberMe != null)
            {
                request.Remember = true;
                Session["RememberLoginCandidate"] = JsonConvert.SerializeObject(request);
            }
            else
            {
                Session["RememberLoginCandidate"] = "";
            }
            TempData["XMessage"] = new XMessage("Notification", "Login Successfull", EnumCategoryMess.success);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["CandidateTest"] = "";
            Session["DisplayNameCandidate"] = "";
            Session["ImageCandidate"] = "";
            return RedirectToAction("Index");
        }
    }
}