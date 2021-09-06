using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class TransferController : Controller
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferController(ITransferRepository transferRepository, IUnitOfWork unitOfWork)
        {
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ListpageSize = new List<int>() { 10, 15, 20, 50, 100 };
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _transferRepository.GetData(keyword, page, pageSize);

            var json = Json(new
            {
                data = result.Items,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        [HttpPost]
        public JsonResult Locked(int id)
        {
            var title = "Notification";
            var cadi = _transferRepository.Locked(id);

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
    }
}