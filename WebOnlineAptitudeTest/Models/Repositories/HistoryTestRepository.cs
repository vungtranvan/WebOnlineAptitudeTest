﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Enums;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface IHistoryTestRepository : IRepository<HistoryTest>
    {
        bool InsertOrUpdate(HisToryTestInsertOrUpdateModel historyTest);
        PagingModel<HistoryTestViewModel> GetData(string keyword, int page, int pageSize);
        bool Locked(int candidateId);

        void UpdateCandidateQuit();
    }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="historyTest">entities | historyTest.TypeAction == 0 ? "Insert" : "Update"</param>
        /// <returns></returns>
        public bool InsertOrUpdate(HisToryTestInsertOrUpdateModel historyTest)
        {
            //foreach (var item in _categoryExamRepository.Get())
            //{
            //    if (historyTest.TypeAction == 0)
            //    {
            //        var model = new HistoryTest()
            //        {
            //            CandidateId = historyTest.CandidateId,
            //            TestEndSchedule = (System.DateTime)historyTest.TestEndSchedule,
            //            TestStartSchedule = historyTest.TestStartSchedule,
            //            TimeTest = historyTest.TimeTest,
            //            CategoryExamId = item.Id,
            //            Deleted = false,
            //            Status = (int)EnumStatusHistoryTest.Undone
            //        };
            //        base.Add(model);
            //    }
            //    else
            //    {
            //        var history = base.Get(filter: h => h.CategoryExamId == item.Id && h.CandidateId == historyTest.CandidateId).FirstOrDefault();
            //        if (history == null)
            //            return false;

            //        history.TestStartSchedule = historyTest.TestStartSchedule;
            //        history.TestEndSchedule = (System.DateTime)historyTest.TestEndSchedule;
            //        history.TimeTest = historyTest.TimeTest;
            //        base.Update(history);
            //    }
            //};

            // Update Status Candidate
            if (historyTest.TypeAction == 0)
            {
                var candi = _candidateRepository.GetSingleById(historyTest.CandidateId);
                candi.Status = true;
            }
            _unitOfWork.Commit();
            return true;
        }

        public bool Locked(int candidateId)
        {
            var history = base.Get(filter: h => h.CandidateId == candidateId);
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

        public void UpdateCandidateQuit()
        {
            //var lst = base.DbContext.HistoryTests.Where(x => (x.Status == (int)EnumStatusHistoryTest.Undone 
            //|| x.Status == (int)EnumStatusHistoryTest.InProgress)
            //&& x.TestEndSchedule < DateTime.Now);
            //if (lst.Count() > 0)
            //{
            //    foreach (var item in lst)
            //    {
            //        item.Status = (int)EnumStatusHistoryTest.Quit;
            //    }
            //    _unitOfWork.Commit();
            //}
        }
    }
}