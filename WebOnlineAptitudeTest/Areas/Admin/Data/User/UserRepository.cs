using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.User
{
    public class UserRepository : IUserRepository
    {
        private OnlineTestDbContext _context;

        public UserRepository()
        {
            _context = new OnlineTestDbContext();
        }

        public Models.Entities.Admin GetByUserName(string userName)
        {
           var user = _context.Admins.Where(x => x.UserName.Equals(userName)).FirstOrDefault();
            if (user==null)
            {
                return null;
            }
            return user;
        }
    }
}