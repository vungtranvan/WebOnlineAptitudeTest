using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class HistoryTestDetailRepository : RepositoryBase<HistoryTestDetail>, IHistoryTestDetailRepository
    {
        public HistoryTestDetailRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}