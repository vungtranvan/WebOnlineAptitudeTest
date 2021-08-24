using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private IAdminRepository _adminRepository;
        private IUnitOfWork _unitOfWork;
        public AccountController(IAdminRepository adminRepository, IUnitOfWork unitOfWork)
        {
            _adminRepository = adminRepository;
            _unitOfWork = unitOfWork;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassword(string userName, string passOld, string passNew)
        {
            var message = "Change Password Successfull !!!";
            var title = "Notification";

            var result = ChangePass(userName.Trim(), passOld.Trim(), passNew.Trim());
          
            if (result == false)
            {
                message = "Change Password Error !!!";
            }

            return Json(new
            {
                message,
                status = result,
                title
            });
        }

        private bool ChangePass(string userName, string passOld, string passNew)
        {
            var user = _adminRepository.Get(filter: x => x.UserName.Equals(userName)).FirstOrDefault();
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