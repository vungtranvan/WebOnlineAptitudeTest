using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.TestSchedules;
using WebOnlineAptitudeTest.Enums;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class TestScheduleRepository : RepositoryBase<TestSchedule>, ITestScheduleRepository
    {
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TestScheduleRepository(IDbFactory dbFactory,
            ICategoryExamRepository categoryExamRepository,
            IHistoryTestRepository historyTestRepository,
            ICandidateRepository candidateRepository,
            IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _categoryExamRepository = categoryExamRepository;
            _candidateRepository = candidateRepository;
            _historyTestRepository = historyTestRepository;
            _unitOfWork = unitOfWork;
        }

        public bool CheckStatus(int id)
        {
            var data = base.GetSingleById(id);
            if (data == null)
                return false;

            if (data.Status == (int)EnumStatusHistoryTest.Undone)
                return true;

            return false;
        }

        public PagingModel<TestSchedule> GetData(string keyword, int page, int pageSize)
        {
            UpdateStatusTestSchedule();
            IEnumerable<TestSchedule> lstData = new List<TestSchedule>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstData = Get(filter: c => c.Deleted == false && (c.Name.ToLower().Contains(keyword.ToLower())),
                              orderBy: c => c.OrderByDescending(x => x.Id)).Select(x => new TestSchedule
                              {
                                  Id = x.Id,
                                  Name = x.Name,
                                  Status = x.Status,
                                  DateStart = x.DateStart,
                                  DateEnd = x.DateEnd,
                                  CreatedDate = x.CreatedDate,
                                  UpdatedDate = x.UpdatedDate
                              }).ToList();
            }
            else
            {
                lstData = Get(filter: c => c.Deleted == false, orderBy: c => c.OrderByDescending(x => x.Id))
                        .Select(x => new TestSchedule
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Status = x.Status,
                            DateStart = x.DateStart,
                            DateEnd = x.DateEnd,
                            CreatedDate = x.CreatedDate,
                            UpdatedDate = x.UpdatedDate
                        }).ToList();
            }

            int totalRow = lstData.Count();

            var data = lstData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<TestSchedule>()
            {
                TotalRow = totalRow,
                Items = data
            };
        }

        public TestScheduleInsertOrUpdateRequest GetInsertOrUpdateRequest(int id)
        {
            var testSchedule = base.GetSingleById(id);
            var lstCandidateId = this.GetListCandidateIdInHistoryTestByTestScheduleId(id);

            var model = new TestScheduleInsertOrUpdateRequest()
            {
                Id = testSchedule.Id,
                Name = testSchedule.Name,
                DateStart = testSchedule.DateStart,
                DateEnd = testSchedule.DateEnd,
                CandidateId = lstCandidateId
            };
            return model;
        }

        public bool InsertOrUpdate(TestScheduleInsertOrUpdateRequest model)
        {
            List<HistoryTest> lstHistoryTest = new List<HistoryTest>();
            foreach (var item in _categoryExamRepository.Get())
            {
                foreach (var c in model.CandidateId)
                {
                    lstHistoryTest.Add(new HistoryTest()
                    {
                        CategoryExamId = item.Id,
                        Deleted = false,
                        Status = (int)EnumStatusHistoryTest.Undone,
                        CandidateId = c
                    });
                }
            }

            if (model.Id == 0)
            {
                var data = new TestSchedule()
                {
                    Name = model.Name,
                    DateStart = model.DateStart,
                    DateEnd = model.DateEnd,
                    CreatedDate = DateTime.Now,
                    Deleted = false,
                    Status = (int)EnumStatusTestSchedule.Undone,
                    HistoryTests = lstHistoryTest
                };
                base.Add(data);
            }
            else
            {
                var t = base.GetSingleById(model.Id);
                if (t == null)
                    return false;

                var lstCandidateIdOld = this.GetListCandidateIdInHistoryTestByTestScheduleId(model.Id);
                if (model.CandidateId.Except(lstCandidateIdOld).Count() > 0 || lstCandidateIdOld.Except(model.CandidateId).Count() > 0)
                {
                    // delete HistoryTest old
                    foreach (var item in lstCandidateIdOld)
                    {
                        _historyTestRepository.DeleteMulti(x => x.CandidateId.Equals(item));
                        var candi = _candidateRepository.GetSingleById(item);
                        candi.Status = false;
                    }

                    t.HistoryTests = lstHistoryTest;
                }

                t.Name = model.Name;
                t.DateStart = model.DateStart;
                t.DateEnd = model.DateEnd;
                t.UpdatedDate = DateTime.Now;

                base.Update(t);
            }

            // Update Status Candidate
            foreach (var item in model.CandidateId)
            {
                var candi = _candidateRepository.GetSingleById(item);
                candi.Status = true;
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

            // Update Status Candidate
            var lstCandidateIdOld = this.GetListCandidateIdInHistoryTestByTestScheduleId(id);
            foreach (var item in lstCandidateIdOld)
            {
                var candi = _candidateRepository.GetSingleById(item);
                candi.Status = false;
            }

            _unitOfWork.Commit();
            return true;
        }

        public void UpdateStatusTestSchedule()
        {
            var lst1 = base.GetMulti(x => x.DateEnd < DateTime.Now, new string[] { "HistoryTests" }).ToList();

            if (lst1.Count() > 0)
            {
                foreach (TestSchedule item in lst1)
                {
                    item.Status = (int)EnumStatusTestSchedule.Done;
                    _unitOfWork.Commit();
                }
            }

            //var lst2 = base.GetMulti(x => x.DateStart > DateTime.Now && x.DateEnd > DateTime.Now
            //           && x.Status == (int)EnumStatusTestSchedule.Undone, new string[] { "HistoryTests" }).ToList();

            //if (lst2.Count() > 0)
            //{
            //    foreach (TestSchedule item in lst2)
            //    {
            //        item.Status = (int)EnumStatusTestSchedule.InProgress;
            //    }
            //    _unitOfWork.Commit();
            //}
        }

        public List<int> GetListCandidateIdInHistoryTestByTestScheduleId(int id)
        {
            var data = (from x in base.DbContext.HistoryTests
                        join c in base.DbContext.Candidates on x.CandidateId equals c.Id
                        where x.Deleted == false && x.TestScheduleId.Equals(id) && c.Deleted == false
                        select new
                        {
                            CandidateId = x.CandidateId,
                        }).GroupBy(x => x.CandidateId).Select(y => y.FirstOrDefault());

            List<int> lstCandidateId = new List<int>();
            foreach (var item in data)
            {
                lstCandidateId.Add(item.CandidateId);
            }
            return lstCandidateId;
        }
    }
}