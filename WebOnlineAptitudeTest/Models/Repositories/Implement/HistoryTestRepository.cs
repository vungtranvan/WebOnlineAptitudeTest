using System;
using System.Collections.Generic;
using System.Linq;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Enums;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class HistoryTestRepository : RepositoryBase<HistoryTest>, IHistoryTestRepository
    {
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HistoryTestRepository(IDbFactory dbFactory,
            ICategoryExamRepository categoryExamRepository,
            ICandidateRepository candidateRepository,
            IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _categoryExamRepository = categoryExamRepository;
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        public PagingModel<HistoryTestViewModel> GetData(string keyword, int page, int pageSize)
        {
            //UpdateCandidateQuit();
            //IEnumerable<HistoryTestViewModel> lstHistory = new List<HistoryTestViewModel>();

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    lstHistory = (from x in base.DbContext.HistoryTests
            //                  join c in base.DbContext.Candidates on x.CandidateId equals c.Id
            //                  where x.Deleted == false && c.Deleted == false && c.Name.ToLower().Contains(keyword.ToLower())
            //                  orderby x.Status
            //                  select new HistoryTestViewModel
            //                  {
            //                      CandidateId = x.CandidateId,
            //                      CandidateName = c.Name,
            //                      TestStartSchedule = x.TestStartSchedule,
            //                      TestEndSchedule = x.TestEndSchedule,
            //                      Status = x.Status
            //                  }).ToList().GroupBy(x => x.CandidateId).Select(y => y.FirstOrDefault());
            //}
            //else
            //{
            //    lstHistory = (from x in base.DbContext.HistoryTests
            //                  join c in base.DbContext.Candidates on x.CandidateId equals c.Id
            //                  where x.Deleted == false && c.Deleted == false
            //                  orderby x.Status
            //                  select new HistoryTestViewModel
            //                  {
            //                      CandidateId = x.CandidateId,
            //                      CandidateName = c.Name,
            //                      TestStartSchedule = x.TestStartSchedule,
            //                      TestEndSchedule = x.TestEndSchedule,
            //                      Status = x.Status
            //                  }).ToList().GroupBy(x => x.CandidateId).Select(y => y.FirstOrDefault());
            //}

            //int totalRow = lstHistory.Count();

            //var data = lstHistory.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            //return new PagingModel<HistoryTestViewModel>()
            //{
            //    TotalRow = totalRow,
            //    Items = data
            //};
            return null;
        }
       
        public bool Locked(int candidateId)
        {
            var history = Get(filter: h => h.CandidateId == candidateId);
            if (history.Count() == 0)
            {
                return false;
            }
            foreach (var item in history)
            {
                item.Deleted = true;
            }

            // Update Status Candidate
            var candi = _candidateRepository.GetSingleById(candidateId);
            candi.Status = false;

            _unitOfWork.Commit();
            return true;
        }
    }
}