using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.DAL;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Services.User
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

        public Models.Entities.Admin GetByUserName(string userName)
        {
            var user = new Models.Entities.Admin();
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}