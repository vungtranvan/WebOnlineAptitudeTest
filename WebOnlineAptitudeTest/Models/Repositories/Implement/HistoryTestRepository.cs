﻿using System;
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
        private readonly ITestScheduleRepository _testScheduleRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HistoryTestRepository(IDbFactory dbFactory,
            ICandidateRepository candidateRepository,
            ITestScheduleRepository testScheduleRepository,
            IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _candidateRepository = candidateRepository;
            _testScheduleRepository = testScheduleRepository;
            _unitOfWork = unitOfWork;
        }

        public PagingModel<Candidate> GetData(string keyword, int idTeschedule, int page, int pageSize)
        {
            UpdateStatus();
            List<Candidate> lstHistoryTest = new List<Candidate>();

            if (idTeschedule > 0)
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone
                                     && (x.Name.Contains(keyword) || x.UserName.Contains(keyword))
                                     && x.HistoryTests.Where(y => y.TestScheduleId.Equals(idTeschedule)).Count() > 0, new string[] { "HistoryTests" })
                                    .Select(c => new Candidate()
                                    {
                                        Id = c.Id,
                                        Name = c.Name,
                                        Image = c.Image,
                                        Email = c.Email,
                                        HistoryTests = c.HistoryTests,
                                        UserName = c.UserName,
                                        Status = c.Status,
                                        ToTalMark = (double)c.HistoryTests.Select(x => x.PercentMark).Sum() / 3
                                    }).ToList();
                }
                else
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone
                    && x.HistoryTests.Where(y => y.TestScheduleId.Equals(idTeschedule)).Count() > 0, new string[] { "HistoryTests" })
                        .Select(c => new Candidate()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Image = c.Image,
                            Email = c.Email,
                            HistoryTests = c.HistoryTests,
                            UserName = c.UserName,
                            Status = c.Status,
                            ToTalMark = (double)c.HistoryTests.Select(x => x.PercentMark).Sum() / 3
                        }).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone
                    && (x.Name.Contains(keyword) || x.UserName.Contains(keyword)), new string[] { "HistoryTests" })
                        .Select(c => new Candidate()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Image = c.Image,
                            Email = c.Email,
                            HistoryTests = c.HistoryTests,
                            UserName = c.UserName,
                            Status = c.Status,
                            ToTalMark = (double)c.HistoryTests.Select(x => x.PercentMark).Sum() / 3
                        }).ToList();
                }
                else
                {
                    lstHistoryTest = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status != EnumStatusCandidate.Undone, new string[] { "HistoryTests" }).Select(c => new Candidate()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Image = c.Image,
                        Email = c.Email,
                        HistoryTests = c.HistoryTests,
                        UserName = c.UserName,
                        Status = c.Status,
                        ToTalMark = (double)c.HistoryTests.Select(x => x.PercentMark).Sum() / 3
                    }).ToList();
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
            candi.Status = EnumStatusCandidate.Undone;

            _unitOfWork.Commit();
            return true;
        }

        public void UpdateStatus()
        {
            //var lstCandi = _candidateRepository.GetMulti(x => x.Deleted == false && x.Status == EnumStatusCandidate.Scheduled, new string[] { "HistoryTests" });

            //foreach (var item in lstCandi)
            //{

            //    var lstDataInProgress = base.GetMulti(x => x.Deleted == false &&x.CandidateId.Equals(item.Id) && x.Status == EnumStatusHistoryTest.InProgress
            //               && x.TestSchedule.DateEnd < DateTime.Now, new string[] { "TestSchedule" });
            //    foreach (var h in lstDataInProgress)
            //    {
            //        h.Status = EnumStatusHistoryTest.Done;
            //        item.Status = EnumStatusCandidate.Done;
            //    }

            //    var lstDataUndone = base.GetMulti(x => x.Deleted == false && x.CandidateId.Equals(item.Id) && x.Status == EnumStatusHistoryTest.Undone
            //              && x.TestSchedule.DateEnd < DateTime.Now, new string[] { "TestSchedule" });
            //    foreach (var h in lstDataUndone)
            //    {

            //        if (item.HistoryTests.Where(y => y.DateStartTest == null).Count() > 3)
            //        {
            //            item.Status = EnumStatusCandidate.Quit;
            //        }
            //    }
            //}
            _unitOfWork.Commit();
        }
    }
}