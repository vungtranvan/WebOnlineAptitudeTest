using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface ITransferRepository : IRepository<Transfer>
    {
    }
    public class TransferRepository : RepositoryBase<Transfer>, ITransferRepository
    {
        public TransferRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}