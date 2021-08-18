using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Services.User
{
    public interface IUserRepository
    {
        Models.Entities.Admin GetByUserName(string userName);
    }
}
