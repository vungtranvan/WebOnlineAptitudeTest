using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Enums;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class TestScheduleRepository : RepositoryBase<TestSchedule>, ITestScheduleRepository
    {
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TestScheduleRepository(IDbFactory dbFactory,
            ICategoryExamRepository categoryExamRepository,
            ICandidateRepository candidateRepository,
            IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _categoryExamRepository = categoryExamRepository;
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        public PagingModel<TestSchedule> GetData(string keyword, int page, int pageSize)
        {
            UpdateCandidateQuit();
            IEnumerable<TestSchedule> lstData = new List<TestSchedule>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstData = Get(filter: c => c.Deleted == false && (c.Name.ToLower().Contains(keyword.ToLower())),
                              orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }
            else
            {
                lstData = Get(filter: c => c.Deleted == false, orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }

            int totalRow = lstData.Count();

            var data = lstData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<TestSchedule>()
            {
                TotalRow = totalRow,
                Items = data
            };
        }

        public bool InsertOrUpdate(TestSchedule testSchedule)
        {
            foreach (var item in _categoryExamRepository.Get())
            {
                if (testSchedule.Id == 0)
                {
                    testSchedule.Deleted = false;
                    testSchedule.Status = (int)EnumStatusHistoryTest.Undone;
                    base.Add(testSchedule);
                }
                else
                {
                    var t = base.GetMulti(x => x.Id.Equals(testSchedule.Id), new string[] { "HistoryTests" }).FirstOrDefault();
                    if (t == null)
                        return false;

                    t.DateStart = testSchedule.DateStart;
                    t.DateEnd = testSchedule.DateEnd;
                    t.TimeTest = testSchedule.TimeTest;
                    t.UpdatedDate = DateTime.Now;
                    base.Update(t);
                }
            };

            // Update Status Candidate
            if (testSchedule.Id == 0)
            {
                //var candi = _candidateRepository.GetSingleById(testSchedule.CandidateId);
                //candi.Status = true;
            }
            _unitOfWork.Commit();
            return true;
        }

        public bool Locked(int id)
        {
            var t = GetSingleById(id);
            if (t == null)
            {
                return false;
            }
            t.Deleted = true;

            _unitOfWork.Commit();
            return true;
        }

        public void UpdateCandidateQuit()
        {
            var lst = base.GetMulti(x => x.DateEnd < DateTime.Now, new string[] { "HistoryTests" });

            if (lst.Count() > 0)
            {
                foreach (TestSchedule item in lst)
                {
                    if (item.HistoryTests.Count > 0)
                    {
                        foreach (var h in item.HistoryTests)
                        {
                            if (h.Status == (int)EnumStatusHistoryTest.Undone || h.Status == (int)EnumStatusHistoryTest.InProgress)
                            {
                                item.Status = (int)EnumStatusHistoryTest.Quit;
                            }
                        }
                    }
                }
                _unitOfWork.Commit();
            }
        }
    }
}