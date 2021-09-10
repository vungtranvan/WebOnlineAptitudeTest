using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.TestSchedules;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories.Interface
{
    public interface ITestScheduleRepository : IRepository<TestSchedule>
    {
        bool InsertOrUpdate(TestScheduleInsertOrUpdateRequest model);
        PagingModel<TestSchedule> GetData(string keyword, int page, int pageSize);
        bool Locked(int id);

        void UpdateStatusTestSchedule();

        TestScheduleInsertOrUpdateRequest GetInsertOrUpdateRequest(int id);
        PagingModel<dynamic> CountCandidate(DateTime? fromDate, DateTime? toDate, int page, int pageSize);
        bool CheckStatus(int id);
    }
}