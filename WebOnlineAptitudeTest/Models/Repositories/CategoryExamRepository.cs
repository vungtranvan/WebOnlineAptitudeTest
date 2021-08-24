using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface ICategoryExamRepository : IRepository<CategoryExam>
    {
    }
    public class CategoryExamRepository : RepositoryBase<CategoryExam>, ICategoryExamRepository
    {
        public CategoryExamRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}