using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Areas.Admin.Data.Services.Cadidates;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CadidateController : Controller //BaseController
    {
        private ICadidateService _cadidateService;
        public CadidateController()
        {
            _cadidateService = new CadidateService();
        }

        public CadidateController(ICadidateService cadidateService)
        {
            _cadidateService = cadidateService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var data = _cadidateService.Get();
            return View(data);
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

            if (result == false)
            {
                return HttpNotFound();
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
            return RedirectToAction("Index");
        }
    }
}
