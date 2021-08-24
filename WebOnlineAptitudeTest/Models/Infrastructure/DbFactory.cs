using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Models.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private OnlineTestDbContext dbContext;

        public OnlineTestDbContext Init()
        {
            return dbContext ?? (dbContext = new OnlineTestDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}