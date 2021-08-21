using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Areas.Admin.Data.Services.Candidates;
using System;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CandidateController : BaseController
    {
        private ICadidateService _cadidateService;
        public CandidateController()
        {
            _cadidateService = new CadidateService();
        }

        public CandidateController(ICadidateService cadidateService)
        {
            _cadidateService = cadidateService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var c = 234;
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            var d = 345;
            var result = _cadidateService.Get(keyword, page, pageSize);

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
            var candidate = _cadidateService.Get(id);

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
        public ActionResult InsertOrUpdate(int id)
        {
            var candidate = _cadidateService.Get(id);
            if (id != 0)
            {
                candidate.Password = "";
            }
            return View(candidate);
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

                if (_cadidateService.CheckExitEmail(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (_cadidateService.CheckExitPhone(candidate.Phone) && candidate.Phone != null)
                    ModelState.AddModelError("Phone", "Phone already exists !!!");
                if (_cadidateService.CheckExitUserName(candidate.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }
            else
            {
                var c = _cadidateService.Get(idCandi);

                if (c == null)
                    return HttpNotFound();

                if (_cadidateService.CheckExitEmail(candidate.Email) && !c.Email.Equals(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (c.Phone != null)
                {
                    if (_cadidateService.CheckExitPhone(candidate.Phone) && !c.Phone.Equals(candidate.Phone))
                        ModelState.AddModelError("Phone", "Phone already exists !!!");
                }
                if (_cadidateService.CheckExitUserName(candidate.UserName) && !c.UserName.Equals(candidate.UserName))
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

            var result = _cadidateService.InsertOrUpdate(candidate);

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
            var result = _cadidateService.Delete(id);
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
