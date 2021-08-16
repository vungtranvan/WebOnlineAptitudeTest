using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.User;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private IUserRepository _userRepository;

        public AuthController()
        {
            _userRepository = new UserRepository();
        }

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (!Session["UserAdmin"].Equals(""))
            {
                return RedirectToAction("Index", "Homes");
            }
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string password, string rememberMe)
        {
            var user = _userRepository.GetByUserName(userName);
            if (user == null)
            {
                ViewBag.Error = "UserName does not exist!!!";
            }
            else
            {
                if (user.Password.Equals(MyString.ToMD5(password)))
                {
                    Session["UserAdmin"] = user.UserName;
                    Session["DisplayNameAdmin"] = user.DisplayName;
                    Session["ImageAdmin"] = user.Image;
                    ViewBag.Error = "";
                    return RedirectToAction("Index", "Homes");
                }
                else
                {
                    ViewBag.Error = "Incorrect password!!!";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["UserAdmin"] = "";
            Session["DisplayNameAdmin"] = "";
            Session["ImageAdmin"] = "";
            return RedirectToAction("Index", "Homes");
        }
    }
}