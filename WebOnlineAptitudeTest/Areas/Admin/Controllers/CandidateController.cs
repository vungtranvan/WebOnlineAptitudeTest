using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Areas.Admin.Data.Services.Candidates;

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
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            var result = _cadidateService.Get(keyword, page, pageSize);

            return Json(new
            {
                data = result.Items,
                totalRow= result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var candidate = _cadidateService.Get(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
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
            if (candidate.Id == 0)
            {
                if (string.IsNullOrEmpty(candidate.Password))
                    ModelState.AddModelError("Password", "This field is required");

                if (_cadidateService.CheckExitEmail(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (_cadidateService.CheckExitPhone(candidate.Phone))
                    ModelState.AddModelError("Phone", "Phone already exists !!!");
                if (_cadidateService.CheckExitUserName(candidate.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }
            else
            {
                var c = _cadidateService.Get(candidate.Id);

                if (c == null)
                    return HttpNotFound();

                if (_cadidateService.CheckExitEmail(candidate.Email) && !c.Email.Equals(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (_cadidateService.CheckExitPhone(candidate.Phone) && !c.Phone.Equals(candidate.Phone))
                    ModelState.AddModelError("Phone", "Phone already exists !!!");
                if (_cadidateService.CheckExitUserName(candidate.UserName) && !c.UserName.Equals(candidate.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }

            if (!ModelState.IsValid)
            {
                return View(candidate);
            }

            var result = _cadidateService.InsertOrUpdate(candidate);

            if (result == true)
            {
                if (candidate.Id == 0)
                {
                    TempData["XMessage"] = new XMessage("Notification", "Add Successfull", EnumCategoryMess.success);
                }
                else
                {
                    TempData["XMessage"] = new XMessage("Notification", "Edit Successfull", EnumCategoryMess.success);
                }
            }
            else
            {
                if (candidate.Id == 0)
                {
                    TempData["XMessage"] = new XMessage("Notification", "Add Error", EnumCategoryMess.error);
                }
                else
                {
                    TempData["XMessage"] = new XMessage("Notification", "Edit Error", EnumCategoryMess.error);
                }
            }

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var result = _cadidateService.Delete(id);
            if (result == false)
            {
                return HttpNotFound();
            }
            TempData["XMessage"] = new XMessage("Notification", "Delete Successfull", EnumCategoryMess.success);
            return RedirectToAction("Index");
        }
    }
}
