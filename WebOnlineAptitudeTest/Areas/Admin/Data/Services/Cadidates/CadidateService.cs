﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.DAL;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Services.Cadidates
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
            var cadi = this.Get(id);
            if (cadi == null)
            {
                return false;
            }
            cadi.Deleted = !cadi.Deleted;

            this.SaveAndipose();
            return true;
        }

        public List<Candidate> Get()
        {
            return _unitOfWork.CandidateRepository.Get().ToList();
        }

        public Candidate Get(int id)
        {
            return _unitOfWork.CandidateRepository.GetByID(id);
        }

        public bool InsertOrUpdate(Candidate candidate)
        {
            if (candidate.Id == 0)
            {
                candidate.Password = MyString.ToMD5(candidate.Password);
                candidate.Status = false;
                candidate.CreatedDate = DateTime.Now;
                candidate.Deleted = false;
                _unitOfWork.CandidateRepository.Insert(candidate);
            }
            else
            {
                var cadi = Get(candidate.Id);
                if (cadi == null)
                {
                    return false;
                }
                candidate.CreatedDate = cadi.CreatedDate;
                candidate.UpdatedDate = DateTime.Now;
                if (candidate.Password.Equals(""))
                {
                    candidate.Password = cadi.Password;
                }

                _unitOfWork.CandidateRepository.Update(candidate);
            }

            this.SaveAndipose();
            return true;
        }

        private void SaveAndipose()
        {
            _unitOfWork.Save();
            _unitOfWork.Dispose();
        }
    }
}