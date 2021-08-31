using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories.Interface
{
    public interface ITestScheduleRepository : IRepository<TestSchedule>
    {
        bool InsertOrUpdate(TestSchedule testSchedule);
        PagingModel<TestSchedule> GetData(string keyword, int page, int pageSize);
        bool Locked(int id);

        void UpdateCandidateQuit();
    }
}