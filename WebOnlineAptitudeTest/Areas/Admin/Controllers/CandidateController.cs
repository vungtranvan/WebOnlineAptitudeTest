using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Areas.Admin.Data.DAL.Candidates;
using System;
using System.Collections.Generic;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CandidateController : BaseController
    {
        private ICandidateRepository _candidateRepository;
        public CandidateController()
        {
            _candidateRepository = new CandidateRepository();
        }

        public CandidateController(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
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
            var result = _candidateRepository.Get(keyword, page, pageSize);

            return Json(new
            {
                data = result.Items,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Details(int id)
        {
            var candidate = _candidateRepository.Get(id);

            if (candidate == null)
            {
                return Json(new
                {
                    data = candidate,
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                data = candidate,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertOrUpdate(int? id)
        {
            if (id != null)
            {
                var candidate = _candidateRepository.Get(id.Value);
                candidate.Password = "";
                return View(candidate);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertOrUpdate(Candidate candidate)
        {
            var idCandi = candidate.Id;

            // Validate data input
            if (idCandi == 0)
            {
                if (string.IsNullOrEmpty(candidate.Password))
                    ModelState.AddModelError("Password", "This field is required");

                if (_candidateRepository.CheckExitEmail(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (_candidateRepository.CheckExitPhone(candidate.Phone) && candidate.Phone != null)
                    ModelState.AddModelError("Phone", "Phone already exists !!!");
                if (_candidateRepository.CheckExitUserName(candidate.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }
            else
            {
                var c = _candidateRepository.Get(idCandi);

                if (c == null)
                    return HttpNotFound();

                if (_candidateRepository.CheckExitEmail(candidate.Email) && !c.Email.Equals(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (c.Phone != null)
                {
                    if (_candidateRepository.CheckExitPhone(candidate.Phone) && !c.Phone.Equals(candidate.Phone))
                        ModelState.AddModelError("Phone", "Phone already exists !!!");
                }
                if (_candidateRepository.CheckExitUserName(candidate.UserName) && !c.UserName.Equals(candidate.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");

            }

            if (!ModelState.IsValid)
            {
                return View(candidate);
            }

            // Xử lý ảnh
            if (string.IsNullOrEmpty(candidate.Image))
            {
                candidate.Image = "/Content/default-avatar.jpg";
            }
            else
            {
                string hostUrl = Request.Url.Scheme + "://" + Request.Url.Host;
                if (!candidate.Image.Contains(hostUrl))
                    candidate.Image = hostUrl + candidate.Image;
                candidate.Image = candidate.Image.CutHostAndSchemePathFile();
            }

            var result = _candidateRepository.InsertOrUpdate(candidate);

            if (result == true)
            {
                if (idCandi == 0)
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
                if (idCandi == 0)
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
        public JsonResult Delete(int id)
        {
            var status = true;
            var message = "Delete Successfull !!!";
            var title = "Notification";
            var result = _candidateRepository.Delete(id);
            if (result == false)
            {
                status = false;
                message = "Delete Error !!!";
            }
            return Json(new
            {
                message,
                status,
                title
            });
        }
    }
}
