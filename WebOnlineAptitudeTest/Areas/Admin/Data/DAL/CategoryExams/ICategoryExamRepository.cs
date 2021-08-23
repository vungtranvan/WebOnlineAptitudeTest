using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.DAL.CategoryExams
{
    public interface ICategoryExamRepository
    {
        List<CategoryExam> Get();
        CategoryExam Get(int id);
        bool Update(CategoryExam category);
    }
}
