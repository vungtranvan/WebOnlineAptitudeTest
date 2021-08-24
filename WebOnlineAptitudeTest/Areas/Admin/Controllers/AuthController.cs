using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.User;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class AuthController : Controller
    {
        private IAdminRepository _adminRepository;
        private IUnitOfWork _unitOfWork;

        public AuthController(IAdminRepository adminRepository, IUnitOfWork unitOfWork)
        {
            _adminRepository = adminRepository;
            _unitOfWork = unitOfWork;
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

            var user = _adminRepository.Get(filter: x => x.UserName.Equals(request.UserName) && x.Deleted == false).FirstOrDefault();

            if (user == null)
            {
                ViewBag.Error = "UserName does not exist!!!";
                return View(request);
            }

            if (!user.Password.Equals(MyHelper.ToMD5(request.Password)))
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
            TempData["XMessage"] = new XMessage("Notification", "Login Successfull", EnumCategoryMess.success);
            return RedirectToAction("Index", "Homes");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session["UserAdmin"] = "";
            Session["DisplayNameAdmin"] = "";
            Session["ImageAdmin"] = "";
            return RedirectToAction("Index");
        }
    }
}