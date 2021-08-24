using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface ICandidateRepository : IRepository<Candidate>
    {
    }
    public class CandidateRepository: RepositoryBase<Candidate>, ICandidateRepository
    {
        public CandidateRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}