using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.DAL.User;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private IUserRepository _userRepository;
        public AccountController()
        {
            _userRepository = new UserRepository();
        }
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: Admin/Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ChangePassword(string userName, string passOld, string passNew)
        {
            var message = "Change Password Successfull !!!";
            var title = "Notification";

            var result = _userRepository.ChangePassword(userName.Trim(), passOld.Trim(), passNew.Trim());
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