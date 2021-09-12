using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
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

        public PagingModel<CategoryExam> GetData(string keyword, int page, int pageSize)
        {
            var lstData = new List<CategoryExam>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstData = Get(filter: c => c.Name.ToLower().Contains(keyword.ToLower()), orderBy: c => c.OrderBy(x => x.Id)).ToList();
            }
            else
            {
                lstData = Get(orderBy: c => c.OrderBy(x => x.Id)).ToList();
            }

            int totalRow = lstData.Count();

            var data = lstData.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<CategoryExam>() { TotalRow = totalRow, Items = data };
        }
    }
}