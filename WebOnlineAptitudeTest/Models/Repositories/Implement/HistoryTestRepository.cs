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

        public PagingModel<Candidate> GetData(string keyword, int idTeschedule, int page, int pageSize)
        {
            //UpdateCandidateQuit();
            List<Candidate> lstHistoryTest = new List<Candidate>();

            if (idTeschedule > 0)
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone
                    && (x.Name.Contains(keyword)
                                     || x.UserName.Contains(keyword)) && x.HistoryTests.Where(y => y.TestScheduleId.Equals(idTeschedule)).Count() > 0, new string[] { "HistoryTests" })
                                     .ToList();
                }
                else
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone
                    && x.HistoryTests.Where(y => y.TestScheduleId.Equals(idTeschedule)).Count() > 0, new string[] { "HistoryTests" }).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone
                    && (x.Name.Contains(keyword) || x.UserName.Contains(keyword)), new string[] { "HistoryTests" }).ToList();
                }
                else
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone, new string[] { "HistoryTests" }).ToList();
                }
            }

            int totalRow = lstHistoryTest.Count();

            var data = lstHistoryTest.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Candidate>() { TotalRow = totalRow, Items = data };
        }

        //public int GetCurrentCategoryId()
        //{
           
        //}
       
        public bool Locked(int candidateId)
        {
            //var history = Get(filter: h => h.CandidateId == candidateId);
            //if (history.Count() == 0)
            //{
            //    return false;
            //}
            //foreach (var item in history)
            //{
            //    item.Deleted = true;
            //}

            //// Update Status Candidate
            //var candi = _candidateRepository.GetSingleById(candidateId);
            //candi.Status = EnumStatusCandidate;

            //_unitOfWork.Commit();
            return true;
        }
    }
}