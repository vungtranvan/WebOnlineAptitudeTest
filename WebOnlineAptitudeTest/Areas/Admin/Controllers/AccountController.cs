﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IAdminRepository adminRepository, IUnitOfWork unitOfWork)
        {
            _adminRepository = adminRepository;
            _unitOfWork = unitOfWork;
        }
        
        public ActionResult Index()
        {
            ViewBag.ListpageSize = new List<int>() { 5, 10, 15, 20, 50, 100 };
            return View();
        }

        public JsonResult LoadData(string keyword, int page, int pageSize)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _adminRepository.GetData(keyword, page, pageSize);

            return Json(new
            {
                data = result.Items,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
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

        public JsonResult Details(int id)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var acc = _adminRepository.GetSingleById(id);

            if (acc == null)
            {
                return Json(new
                {
                    data = acc.CreatedDate.ToString(),
                    createdDate =  acc.CreatedDate.ToString(),
                    updatedDate =  acc.UpdatedDate.ToString(),
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                data = acc,
                createdDate = acc.CreatedDate.ToString(),
                updatedDate = acc.UpdatedDate.ToString(),
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertOrUpdate(int? id)
        {
            if (id != null)
            {
                var acc = _adminRepository.GetSingleById(id.Value);
                acc.Password = "";
                return View(acc);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertOrUpdate(Models.Entities.Admin acc)
        {
            acc.UpdatedDate = DateTime.Now;
            var idAcc = acc.Id;

            #region Validate data input
            var checkEmail = _adminRepository.CheckContains(c => c.Email.Equals(acc.Email));
            var checkUserName = _adminRepository.CheckContains(c => c.UserName.Equals(acc.UserName));

            if (idAcc == 0)
            {
                if (string.IsNullOrEmpty(acc.Password))
                    ModelState.AddModelError("Password", "This field is required");

                if (checkEmail)
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (checkUserName)
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }
            else
            {
                var account = _adminRepository.GetSingleById(idAcc);
                if (account == null)
                    return HttpNotFound();

                if (checkEmail && !account.Email.Equals(acc.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (checkUserName && !account.UserName.Equals(acc.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }

            if (!ModelState.IsValid)
            {
                return View(acc);
            }
            #endregion

            #region Processing Image
            if (string.IsNullOrEmpty(acc.Image))
            {
                acc.Image = "/Content/default-avatar.jpg";
            }
            else
            {
                string hostUrl = Request.Url.Scheme + "://" + Request.Url.Host;
                if (!acc.Image.Contains(hostUrl))
                    acc.Image = hostUrl + acc.Image;
                acc.Image = acc.Image.CutHostAndSchemePathFile();
            }
            #endregion

            var result = _adminRepository.InsertOrUpdate(acc);

            if (result == true)
            {
                if (idAcc == 0)
                {
                    TempData["XMessage"] = new XMessage("Notification", "Add Successfull !!!", EnumCategoryMess.success);
                }
                else
                {
                    TempData["XMessage"] = new XMessage("Notification", "Edit Successfull !!!", EnumCategoryMess.success);
                }
            }
            else
            {
                if (idAcc == 0)
                {
                    TempData["XMessage"] = new XMessage("Notification", "Add Error !!!", EnumCategoryMess.error);
                }
                else
                {
                    TempData["XMessage"] = new XMessage("Notification", "Edit Error !!!", EnumCategoryMess.error);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Locked(int id)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var title = "Notification";
            var account = _adminRepository.Locked(id);

            if (!account)
            {
                return Json(new
                {
                    message = "Delete Error !!!",
                    status = false,
                    title
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                message = "Delete Successfull !!!",
                status = true,
                title
            }, JsonRequestBehavior.AllowGet);
        }
    }
}