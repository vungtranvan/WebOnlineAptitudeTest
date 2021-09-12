using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Enums;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
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
                // candidate.Password = candidate.Password.ToMD5();
                //candidate.Password = candidate.Password;
                candidate.Status = EnumStatusCandidate.New;
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
                    //cadi.Password = candidate.Password.ToMD5();
                    cadi.Password = candidate.Password;
                }
                base.Update(cadi);
            }
            _unitOfWork.Commit();
            return true;
        }

        public bool AddMulti(List<Candidate> lstData)
        {
            try
            {
                foreach (var item in lstData)
                {
                    item.Status = EnumStatusCandidate.New;
                    item.CreatedDate = DateTime.Now;
                    item.Deleted = false;
                    base.Add(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                var error = string.Join("-", ex.Message);
                return false;
            }
        }
        public PagingModel<Candidate> GetData(string keyword, int page, int pageSize)
        {
            List<Candidate> lstCandi = new List<Candidate>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstCandi = Get(
                    filter: c => c.Deleted == false && (c.UserName.ToLower().Contains(keyword.ToLower())
                    || c.Name.ToLower().Contains(keyword.ToLower()) || c.Email.ToLower().Contains(keyword.ToLower())),
                    orderBy: c => c.OrderBy(x => x.Status)).ToList();
            }
            else
            {
                lstCandi = Get(filter: c => c.Deleted == false, orderBy: c => c.OrderBy(x => x.Status)).ToList();
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