using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface IHistoryTestDetailRepository : IRepository<HistoryTestDetail>
    {
    }
    public class HistoryTestDetailRepository : RepositoryBase<HistoryTestDetail>, IHistoryTestDetailRepository
    {
        public HistoryTestDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}