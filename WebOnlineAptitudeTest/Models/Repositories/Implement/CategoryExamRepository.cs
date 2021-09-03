using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class CategoryExamRepository : RepositoryBase<CategoryExam>, ICategoryExamRepository
    {
        public CategoryExamRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}