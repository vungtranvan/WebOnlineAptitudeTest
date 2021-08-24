using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Models.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}