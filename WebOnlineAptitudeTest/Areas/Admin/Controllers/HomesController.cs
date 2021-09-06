using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class HomesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            ViewBag.CountTestSchedule = _unitOfWork.DbContext.TestSchedules.Where(x => x.Deleted == false).Count();
            ViewBag.CountAccountAdmin = _unitOfWork.DbContext.Admins.Where(x => x.Deleted == false).Count();
            ViewBag.CountCandidate = _unitOfWork.DbContext.Candidates.Where(x => x.Deleted == false).Count();
            ViewBag.CountQuestion = _unitOfWork.DbContext.Questions.Where(x => x.Deleted == false).Count();
            return View();
        }
    }
}