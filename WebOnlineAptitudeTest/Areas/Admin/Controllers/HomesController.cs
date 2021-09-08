using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Charts;
using WebOnlineAptitudeTest.Helper;
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
            var model = _unitOfWork.DbContext.Database.SqlQuery<ToTalRecordTables>("EXEC PROC_CalcCountElementTable").FirstOrDefault();
            return View(model);
        }

        [HttpGet]
        public JsonResult LoadStatusCandi()
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _unitOfWork.DbContext.Database.SqlQuery<CandidateChartViewModel>("EXEC PROC_CalcCountStatusCandidateByGruop");

            var json = Json(new
            {
                data = result
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }
    }
}