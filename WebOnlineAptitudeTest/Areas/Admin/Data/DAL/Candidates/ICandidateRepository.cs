using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.DAL.Candidates
{
    public interface ICandidateRepository
    {
        PagingModel<Candidate> Get(string keyword, int page, int pageSize);
        Candidate Get(int id);
        bool InsertOrUpdate(Candidate candidate);
        bool Locked(int id);
        bool CheckExitUserName(string userName);
        bool CheckExitEmail(string email);
        bool CheckExitPhone(string phone);
    }
}
