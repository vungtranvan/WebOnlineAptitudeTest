﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.User;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Controllers
{
    public class AuthFrontendController : Controller
    {
        private ICandidateRepository _candidateRepository;
        private IUnitOfWork _unitOfWork;

        public AuthFrontendController(ICandidateRepository candidateRepository, IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
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

            if (!candi.Password.Equals(MyHelper.ToMD5(request.Password)))
            {
                ViewBag.Error = "Incorrect password!!!";
                return View(request);
            }

            Session["CandidateTest"] = candi.UserName;
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