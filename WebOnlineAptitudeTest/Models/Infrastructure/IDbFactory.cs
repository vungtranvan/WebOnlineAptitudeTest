using System;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Models.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        OnlineTestDbContext Init();
    }
}