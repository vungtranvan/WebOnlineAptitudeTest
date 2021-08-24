﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface ICandidateRepository : IRepository<Candidate>
    {
        bool InsertOrUpdate(Candidate candidate);
        bool Locked(int id);
        PagingModel<Candidate> GetData(string keyword, int page, int pageSize);
    }

    public class CandidateRepository : RepositoryBase<Candidate>, ICandidateRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public CandidateRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
        }

        public bool InsertOrUpdate(Candidate candidate)
        {
            if (candidate.Id == 0)
            {
                candidate.Password = candidate.Password.ToMD5();
                candidate.Status = false;
                candidate.CreatedDate = DateTime.Now;
                candidate.Deleted = false;
                base.Add(candidate);
            }
            else
            {
                var cadi = base.GetSingleById(candidate.Id);
                if (cadi == null)
                {
                    return false;
                }
                cadi.UpdatedDate = DateTime.Now;
                cadi.Name = candidate.Name;
                cadi.UserName = candidate.UserName;
                cadi.Email = candidate.Email;
                cadi.Phone = candidate.Phone;
                cadi.Address = candidate.Address;
                cadi.Education = candidate.Education;
                cadi.WorkExperience = candidate.WorkExperience;
                cadi.Birthday = candidate.Birthday;
                cadi.Sex = candidate.Sex;
                cadi.Image = candidate.Image;
                if (candidate.Password != null)
                {
                    cadi.Password = candidate.Password.ToMD5();
                }
                base.Update(cadi);
            }
            _unitOfWork.Commit();
            return true;
        }

        public PagingModel<Candidate> GetData(string keyword, int page, int pageSize)
        {
            List<Candidate> lstCandi = new List<Candidate>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstCandi = base.Get(
                    filter: c => c.Deleted == false && (c.UserName.ToLower().Contains(keyword.ToLower())
                    || c.Name.ToLower().Contains(keyword.ToLower()) || c.Email.ToLower().Contains(keyword.ToLower())),
                    orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }
            else
            {
                lstCandi = base.Get(filter: c => c.Deleted == false, orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }

            int totalRow = lstCandi.Count();

            var data = lstCandi.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Candidate>() { TotalRow = totalRow, Items = data };
        }

        public bool Locked(int id)
        {
            var cadi = base.GetSingleById(id);
            if (cadi == null)
            {
                return false;
            }
            cadi.Deleted = !cadi.Deleted;
            _unitOfWork.Commit();
            return true;
        }
    }
}