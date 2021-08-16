using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.User;
using WebOnlineAptitudeTest.Models.DAL;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private UnitOfWork _unitOfWork;

        public AuthController()
        {
            _unitOfWork = new UnitOfWork();
        }

        [HttpGet]
        public ActionResult Index()
        {
            LoginRequest model = new LoginRequest();

            if (!Session["UserAdmin"].Equals(""))
            {
                return RedirectToAction("Index", "Homes");
            }
            if (!Session["RememberLoginAdmin"].Equals(""))
            {
                model = JsonConvert.DeserializeObject<LoginRequest>(Session["RememberLoginAdmin"].ToString());
            }

            ViewBag.Error = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginRequest request, string rememberMe)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var user = _unitOfWork.AdminRepository.GetByID(request.UserName);

            if (user == null)
            {
                ViewBag.Error = "UserName does not exist!!!";
                return View(request);
            }

            if (!user.Password.Equals(MyString.ToMD5(request.Password)))
            {
                ViewBag.Error = "Incorrect password!!!";
                return View(request);
            }

            Session["UserAdmin"] = user.UserName;
            Session["DisplayNameAdmin"] = user.DisplayName;
            Session["ImageAdmin"] = user.Image;

            if (rememberMe != null)
            {
                request.Remember = true;
                Session["RememberLoginAdmin"] = JsonConvert.SerializeObject(request);
            }
            else
            {
                Session["RememberLoginAdmin"] = "";
            }

            return RedirectToAction("Index", "Homes");
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