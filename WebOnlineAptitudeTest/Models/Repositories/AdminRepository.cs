using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface IAdminRepository : IRepository<Admin>
    {
        bool ChangePass(string userName, string passOld, string passNew);
    }

    public class AdminRepository : RepositoryBase<Admin>, IAdminRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminRepository(IDbFactory dbFactory,IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
        }

        public bool ChangePass(string userName, string passOld, string passNew)
        {
            var user = base.Get(filter: x => x.UserName.Equals(userName)).FirstOrDefault();
            if (!user.Password.Equals(MyHelper.ToMD5(passOld)))
            {
                return false;
            }
            if (user == null || passNew.Length < 3)
                return false;

            user.Password = passNew.ToMD5();
            _unitOfWork.Commit();
            return true;
        }
    }
}