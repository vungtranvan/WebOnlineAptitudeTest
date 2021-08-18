using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Services.Cadidates
{
    public interface ICadidateService
    {
        List<Candidate> Get();
        Candidate Get(int id);
        bool InsertOrUpdate(Candidate candidate);
        bool Delete(int id);
    }
}
