using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.DAL;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.DAL.User
{
    public class UserRepository : IUserRepository
    {
        private UnitOfWork _unitOfWork;

        public UserRepository()
        {
            if (_unitOfWork == null)
            {
                _unitOfWork = new UnitOfWork();
            }
        }

        public bool ChangePassword(string userName, string passOld, string passNew)
        {
            var user = _unitOfWork.AdminRepository.Get(filter: x => x.UserName.Equals(userName)).FirstOrDefault();
            if (!user.Password.Equals(MyHelper.ToMD5(passOld)))
            {
                return false;
            }
            if (user == null || passNew.Length < 3)
                return false;

            user.Password = passNew.ToMD5();
            SaveAndipose();
            return true;
        }

        public Models.Entities.Admin GetByUserName(string userName)
        {
            var user = _unitOfWork.AdminRepository.GetByID(userName);
            if (user == null)
            {
                return null;
            }
            return user;
        }

        private void SaveAndipose()
        {
            _unitOfWork.Save();
            _unitOfWork.Dispose();
        }
    }
}