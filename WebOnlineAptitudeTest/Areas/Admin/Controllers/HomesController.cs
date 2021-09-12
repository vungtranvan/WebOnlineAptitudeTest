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
using WebOnlineAptitudeTest.Models;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;
using WebOnlineAptitudeTest.Models.ViewModels;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    [BackEndAuthorize]
    public class HomesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly ITransferRepository _transferRepository;
        public HomesController(ICandidateRepository candidateRepository,
            IHistoryTestRepository historyTestRepository,
            ITransferRepository transferRepository,
            IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _historyTestRepository = historyTestRepository;
            _transferRepository = transferRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var model = _unitOfWork.DbContext.Database.SqlQuery<ToTalRecordTables>("EXEC PROC_CalcCountElementTable").FirstOrDefault();

            ViewBag.Recents = this._transferRepository.GetTop(5).Select(x => new RecentsCandinade
            {
                Image = x.Candidate.Image,
                Name = x.Candidate.Name,
                Email = x.Candidate.Email,
                ParticipateDate = x.Candidate.CreatedDate,
                TranferDate = x.CreatedDate != null ? x.CreatedDate.Value : DateTime.MinValue,
                GeneralKnowledge = x.Candidate.HistoryTests.Where(y => y.CategoryExamId == 1).FirstOrDefault().PercentMark,
                Mathematics = x.Candidate.HistoryTests.Where(y => y.CategoryExamId == 2).FirstOrDefault().PercentMark,
                ComputerTechnology = x.Candidate.HistoryTests.Where(y => y.CategoryExamId == 3).FirstOrDefault().PercentMark

            }).ToList();

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
            DateTime todayDate = DateTime.Now;
            var countCadinate = this._candidateRepository.GetAll()
                .Where(x => x.Deleted == false && DateTime.Now.AddMonths(-4) <= x.CreatedDate)
                .Select(y => new
                {
                    id = y.Id,
                    CreatedMouth = y.CreatedDate.Month
                })
                .OrderBy(z => z.CreatedMouth)
                .ToList();

            var thisMouthSum = countCadinate.Where(x => x.CreatedMouth == todayDate.Month).ToList();
            var before1MonthSum = countCadinate.Where(x => x.CreatedMouth == todayDate.AddMonths(-1).Month).ToList();
            var before2MonthSum = countCadinate.Where(x => x.CreatedMouth == todayDate.AddMonths(-2).Month).ToList();
            var before3MonthSum = countCadinate.Where(x => x.CreatedMouth == todayDate.AddMonths(-3).Month).ToList();
            var before4MonthSum = countCadinate.Where(x => x.CreatedMouth == todayDate.AddMonths(-4).Month).ToList();


            var resultAll = new
            {
                ThisMouth = thisMouthSum.Count(),
                Before1Month = before1MonthSum.Count(),
                Before2Month = before2MonthSum.Count(),
                Before3Month = before3MonthSum.Count(),
                Before4Month = before4MonthSum.Count(),
                Count = countCadinate.Count()
            };

            var countCandiHis = this._historyTestRepository.GetAll()
                .Where(c => c.Candidate.Deleted == false && DateTime.Now.AddMonths(-4) <= c.TestSchedule.DateEnd)
                .GroupBy(a => a.CandidateId)
                .Select(x => new
                {
                    DateStart = x.FirstOrDefault().TestSchedule.DateStart,
                    DateEnd = x.FirstOrDefault().TestSchedule.DateEnd,
                    Mark = x.Sum(b => b.PercentMark) / 3
                })
                .ToList();



            var thisMouth = countCandiHis.Where(x => x.DateEnd.Month == todayDate.Month).ToList();
            var before1Month = countCandiHis.Where(x => x.DateEnd.Month == todayDate.AddMonths(-1).Month).ToList();
            var before2Month = countCandiHis.Where(x => x.DateEnd.Month == todayDate.AddMonths(-2).Month).ToList();
            var before3Month = countCandiHis.Where(x => x.DateEnd.Month == todayDate.AddMonths(-3).Month).ToList();
            var before4Month = countCandiHis.Where(x => x.DateEnd.Month == todayDate.AddMonths(-4).Month).ToList();

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

                CountSuccess = countCandiHis.Where(x => x.Mark >= 80).Count(),
                CountFailed = countCandiHis.Where(x => x.Mark < 80).Count()
            };



            var json = Json(new
            {
                data = result,
                dataAll = resultAll
            }, JsonRequestBehavior.AllowGet);
            //json.MaxJsonLength = Int32.MaxValue;
            return json;
        }
        private int getPreviousMount(int last)
        {
            var thisMouth = DateTime.Now;
            return thisMouth.AddMonths(-last).Month;
        }
    }
}