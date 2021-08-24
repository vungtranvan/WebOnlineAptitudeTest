using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface IHistoryTestRepository : IRepository<HistoryTest>
    {
    }

    public class HistoryTestRepository : RepositoryBase<HistoryTest>, IHistoryTestRepository
    {
        public HistoryTestRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}