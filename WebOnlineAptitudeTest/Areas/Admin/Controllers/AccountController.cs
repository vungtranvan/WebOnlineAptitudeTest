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
        private readonly IAdminRepository _adminRepository;
        public AccountController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
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

            var result = _adminRepository.ChangePass(userName.Trim(), passOld.Trim(), passNew.Trim());
          
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
    }
}