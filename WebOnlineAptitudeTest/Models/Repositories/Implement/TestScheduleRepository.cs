using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class TestScheduleRepository : RepositoryBase<TestSchedule>, ITestScheduleRepository
    {
        public TestScheduleRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
