using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using System;
using System.Collections.Generic;
using WebOnlineAptitudeTest.Models.Repositories;
using WebOnlineAptitudeTest.Models.Infrastructure;
using System.Linq;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CandidateController : BaseController
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CandidateController(ICandidateRepository candidateRepository, IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        #region Acction
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
            var result = _candidateRepository.GetData(keyword, page, pageSize);

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
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var candidate = _candidateRepository.GetSingleById(id);

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
                var candidate = _candidateRepository.GetSingleById(id.Value);
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

            #region Validate data input
            var checkEmail = _candidateRepository.CheckContains(c => c.Email.Equals(candidate.Email));
            var checkPhone = _candidateRepository.CheckContains(c => c.Phone.Equals(candidate.Phone));
            var checkUserName = _candidateRepository.CheckContains(c => c.UserName.Equals(candidate.UserName));

            if (idCandi == 0)
            {
                if (string.IsNullOrEmpty(candidate.Password))
                    ModelState.AddModelError("Password", "This field is required");

                if (checkEmail)
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (checkPhone && candidate.Phone != null)
                    ModelState.AddModelError("Phone", "Phone already exists !!!");
                if (checkUserName)
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }
            else
            {
                var candi = _candidateRepository.GetSingleById(idCandi);

                if (candi == null)
                    return HttpNotFound();

                if (checkEmail && !candi.Email.Equals(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (candi.Phone != null)
                {
                    if (checkPhone)
                        ModelState.AddModelError("Phone", "Phone already exists !!!");
                }
                if (checkUserName && !candi.UserName.Equals(candidate.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }

            if (!ModelState.IsValid)
            {
                return View(candidate);
            }
            #endregion

            #region Processing Image
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
            #endregion

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
        public JsonResult Locked(int id)
        {
            var title = "Notification";
            var cadi = _candidateRepository.Locked(id);

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
        #endregion
    }
}
