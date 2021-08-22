using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.DAL.User
{
    public interface IUserRepository
    {
        Models.Entities.Admin GetByUserName(string userName);
        bool ChangePassword(string userName, string passOld, string passNew);
    }
}
