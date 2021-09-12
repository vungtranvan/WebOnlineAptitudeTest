using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories.Interface
{
    public interface ICandidateRepository : IRepository<Candidate>
    {
        bool InsertOrUpdate(Candidate candidate);
        bool AddMulti(List<Candidate> lstData);
        bool Locked(int id);
        PagingModel<Candidate> GetData(string keyword, int page, int pageSize);
    }
}
