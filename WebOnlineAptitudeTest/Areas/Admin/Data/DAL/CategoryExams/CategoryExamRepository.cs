using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.DAL;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.DAL.CategoryExams
{
    public class CategoryExamRepository : ICategoryExamRepository
    {

        private UnitOfWork _unitOfWork;

        public CategoryExamRepository()
        {
            if (_unitOfWork == null)
            {
                _unitOfWork = new UnitOfWork();
            }
        }

        public List<CategoryExam> Get()
        {
            return _unitOfWork.CategoryExamRepository.Get().ToList();
        }

        public bool Update(CategoryExam category)
        {
            var cate = _unitOfWork.CategoryExamRepository.GetByID(category.Id);
            if (cate == null)
            {
                return false;
            }
            cate.Name = category.Name;
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