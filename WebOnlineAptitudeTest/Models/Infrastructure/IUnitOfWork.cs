using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Models.Infrastructure
{
    public interface IUnitOfWork
    {
        OnlineTestDbContext DbContext { get; }
        void Commit();
    }
}