using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories.Interface
{
    public interface IHistoryTestRepository : IRepository<HistoryTest>
    {
        PagingModel<Candidate> GetData(string keyword, int idTeschedule, int page, int pageSize);
        bool Locked(int candidateId);
        void UpdateStatusCandidateAndHistoryTest();
    }
}
