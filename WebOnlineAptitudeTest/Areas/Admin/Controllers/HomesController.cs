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
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class HomesController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHistoryTestRepository _historyTestRepository;
        public HomesController(ICandidateRepository candidateRepository,
            IHistoryTestRepository historyTestRepository,
            IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _historyTestRepository = historyTestRepository;
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

        [HttpGet]
        public JsonResult LoadCandiLast5Mounth()
        {
            var countCadinate = this._candidateRepository.GetAll()
                .Where(x => x.Deleted == false && DateTime.Now.AddMonths(-4) <= x.CreatedDate)
                .Select(y => new
                {
                    id = y.Id,
                    CreatedMouth = y.CreatedDate.Month
                })
                .OrderBy(z => z.CreatedMouth)
                .ToList();

            var countPass = this._historyTestRepository.GetAll()
                .Where(c => c.Candidate.Deleted == false && DateTime.Now.AddMonths(-4) <= c.TestSchedule.DateEnd)
                .GroupBy(a => a.CandidateId)
                .Select(x => new
                {
                    DateStart = x.FirstOrDefault().TestSchedule.DateStart,
                    DateEnd = x.FirstOrDefault().TestSchedule.DateEnd,
                    Mark = x.Sum(b => b.PercentMark) / 3
                })
                .ToList();

            DateTime todayDate = DateTime.Now;

            var thisMouth = countPass.Where(x => x.DateEnd.Month == todayDate.Month).ToList();
            var before1Month = countPass.Where(x => x.DateEnd.Month == todayDate.AddMonths(-1).Month).ToList();
            var before2Month = countPass.Where(x => x.DateEnd.Month == todayDate.AddMonths(-2).Month).ToList();
            var before3Month = countPass.Where(x => x.DateEnd.Month == todayDate.AddMonths(-3).Month).ToList();
            var before4Month = countPass.Where(x => x.DateEnd.Month == todayDate.AddMonths(-4).Month).ToList();

            var result = new
            {
                SuccessThisMouth = thisMouth.Where(x => x.Mark >= 80).Count(),
                FailedThisMouth = thisMouth.Where(x => x.Mark <= 80).Count(),
                Successbefore1Month = before1Month.Where(x => x.Mark >= 80).Count(),
                Failedbefore1Month = before1Month.Where(x => x.Mark < 80).Count(),
                Successbefore2Month = before2Month.Where(x => x.Mark >= 80).Count(),
                Failedbefore2Month = before2Month.Where(x => x.Mark < 80).Count(),
                Successbefore3Month = before3Month.Where(x => x.Mark >= 80).Count(),
                Failedbefore3Month = before3Month.Where(x => x.Mark < 80).Count(),
                Successbefore4Month = before4Month.Where(x => x.Mark >= 80).Count(),
                Failedbefore4Month = before4Month.Where(x => x.Mark < 80).Count(),

                CountSuccess = countPass.Where(x => x.Mark >= 80).Count(),
                CountFailed = countPass.Where(x => x.Mark < 80).Count()
            };

            var resultAll = new
            {
                ThisMouth = thisMouth.Where(x => x.Mark >= 80).Count(),
                Before1Month = before1Month.Count(),
                Before2Month = before2Month.Count(),
                Before3Month = before3Month.Count(),
                Before4Month = before4Month.Count(),

                Count = countPass.Where(x => x.Mark >= 80).Count()
            };

            var json = Json(new
            {
                data = result
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }
        private int getPreviousMount(int last)
        {
            var thisMouth = DateTime.Now;
            return thisMouth.AddMonths(-last).Month;
        }
    }
}