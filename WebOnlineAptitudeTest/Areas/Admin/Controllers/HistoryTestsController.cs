using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class HistoryTestsController : Controller
    {
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly IUnitOfWork _unitOfWork;
        public HistoryTestsController(IHistoryTestRepository historyTestRepository, IUnitOfWork unitOfWork)
        {
            _historyTestRepository = historyTestRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index(int? id)
        {
            ViewBag.ListpageSize = new List<int>() { 5, 10, 15, 20, 50, 100 };
            return View();
        }
    }
}