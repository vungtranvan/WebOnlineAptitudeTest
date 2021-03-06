using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.User;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class AdminRepository : RepositoryBase<Admin>, IAdminRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;
        }

        public ResetPasswordResult ChangePass(string userName, string passOld, string passNew)
        {
            var user = base.GetSingleByCondition(x => x.UserName.Equals(userName));
            if (!user.Password.Equals(passOld.ToMD5()))
            {
                return new ResetPasswordResult() { Status = false, Messenger = "Incorrect password old!!!" };
            }
            user.Password = passNew.ToMD5();
            _unitOfWork.Commit();
            return new ResetPasswordResult() { Status = true, Messenger = "Change Password Successfull !!!" };
        }

        public bool InsertOrUpdate(Admin acc)
        {
            if (acc.Id == 0)
            {
                acc.Password = acc.Password.ToMD5();
                acc.CreatedDate = DateTime.Now;
                acc.Deleted = false;
                base.Add(acc);
            }
            else
            {
                var account = base.GetSingleById(acc.Id);
                if (account == null)
                {
                    return false;
                }
                account.UpdatedDate = DateTime.Now;
                account.UserName = acc.UserName;
                account.Email = acc.Email;
                account.Sex = acc.Sex;
                account.Image = acc.Image;
                if (acc.Password != null)
                {
                    account.Password = acc.Password.ToMD5();
                }
                base.Update(account);
            }
            _unitOfWork.Commit();
            return true;
        }

        public PagingModel<Admin> GetData(string keyword, int page, int pageSize)
        {
            List<Admin> lstAcc = new List<Admin>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstAcc = Get(
                    filter: c => c.Deleted == false && (c.UserName.ToLower().Contains(keyword.ToLower())
                    || c.DisplayName.ToLower().Contains(keyword.ToLower()) || c.Email.ToLower().Contains(keyword.ToLower())),
                    orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }
            else
            {
                lstAcc = Get(filter: c => c.Deleted == false, orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }

            int totalRow = lstAcc.Count();

            var data = lstAcc.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Admin>() { TotalRow = totalRow, Items = data };
        }

        public bool Locked(int id)
        {
            var acc = base.GetSingleById(id);
            if (acc == null)
            {
                return false;
            }
            acc.Deleted = !acc.Deleted;
            _unitOfWork.Commit();
            return true;
        }

    }
}
