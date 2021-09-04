using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.User;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories.Interface
{
    public interface IAdminRepository : IRepository<Admin>
    {
        ResetPasswordResult ChangePass(string userName, string passOld, string passNew);
        bool InsertOrUpdate(Admin acc);
        bool Locked(int id);
        PagingModel<Admin> GetData(string keyword, int page, int pageSize);
    }
}
