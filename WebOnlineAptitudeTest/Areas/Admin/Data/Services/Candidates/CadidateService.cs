using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.DAL;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Services.Candidates
{
    public class CadidateService : ICadidateService
    {
        private UnitOfWork _unitOfWork;

        public CadidateService()
        {
            if (_unitOfWork == null)
            {
                _unitOfWork = new UnitOfWork();
            }
        }

        public bool Delete(int id)
        {
            var cadi = Get(id);
            if (cadi == null)
            {
                return false;
            }
            cadi.Deleted = !cadi.Deleted;

            SaveAndipose();
            return true;
        }

        public PagingModel<Candidate> Get(string keyword, int page, int pageSize)
        {
            List<Candidate> lstCandi = new List<Candidate>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstCandi = _unitOfWork.CandidateRepository.Get(
                    filter: c => c.Deleted == false && (c.UserName.ToLower().Contains(keyword.ToLower())
                    || c.Name.ToLower().Contains(keyword.ToLower()) || c.Email.ToLower().Contains(keyword.ToLower())
                    || c.Phone.ToLower().Contains(keyword.ToLower())),
                    orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }
            else
            {
                lstCandi = _unitOfWork.CandidateRepository.Get(filter: c => c.Deleted == false, orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }

            int totalRow = lstCandi.Count();

            var data = lstCandi.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Candidate>() { TotalRow = totalRow, Items = data };

        }

        public Candidate Get(int id)
        {
            return _unitOfWork.CandidateRepository.GetByID(id);
        }

        public bool CheckExitUserName(string userName)
        {
            var data = _unitOfWork.CandidateRepository.Get(filter: c => c.UserName.Equals(userName)).FirstOrDefault();

            if (data == null)
                return false;

            return true;
        }

        public bool CheckExitEmail(string email)
        {
            var data = _unitOfWork.CandidateRepository.Get(filter: c => c.Email.Equals(email)).FirstOrDefault();
            if (data == null)
                return false;
            return true;
        }
        public bool CheckExitPhone(string phone)
        {
            var data = _unitOfWork.CandidateRepository.Get(filter: c => c.Phone.Equals(phone)).FirstOrDefault();
            if (data == null)
                return false;
            return true;
        }

        public bool InsertOrUpdate(Candidate candidate)
        {
            if (candidate.Id == 0)
            {
                candidate.Password = candidate.Password.ToMD5();
                candidate.Status = false;
                candidate.CreatedDate = DateTime.Now;
                candidate.Deleted = false;
                _unitOfWork.CandidateRepository.Insert(candidate);
            }
            else
            {
                //var cadi = Get(candidate.Id);
                //if (cadi == null)
                //{
                //    return false;
                //}

                //candidate.CreatedDate = cadi.CreatedDate;
                //candidate.UpdatedDate = DateTime.Now;
                //if (candidate.Password == null)
                //{
                //    candidate.Password = cadi.Password;
                //}
                //else
                //{
                //    candidate.Password = candidate.Password.ToMD5();
                //}

                //_unitOfWork.CandidateRepository.Update(candidate);

                var cadi = Get(candidate.Id);
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
                if (candidate.Password != null)
                {
                    cadi.Password = candidate.Password.ToMD5();
                }
            }

            SaveAndipose();
            return true;
        }

        private void SaveAndipose()
        {
            _unitOfWork.Save();
            _unitOfWork.Dispose();
        }
    }
}