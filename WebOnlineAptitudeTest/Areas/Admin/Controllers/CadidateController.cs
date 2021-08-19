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
        public  ActionResult Index()
        {
            var data = _cadidateService.Get();
            return View(data);
        }

        [HttpGet]
        public  ActionResult Details(int id)
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
            if (id!=0)
            {
                candidate.Password = "";
            }
            return View(candidate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult InsertOrUpdate(Candidate candidate)
        {
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
        public  ActionResult Delete(int id)
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
