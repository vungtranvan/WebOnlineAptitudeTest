using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using System;
using System.Collections.Generic;
using WebOnlineAptitudeTest.Models.Repositories;
using WebOnlineAptitudeTest.Models.Infrastructure;
using System.Linq;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CandidateController : BaseController
    {
        private ICandidateRepository _candidateRepository;
        private IUnitOfWork _unitOfWork;

        public CandidateController(ICandidateRepository candidateRepository, IUnitOfWork unitOfWork)
        {
            _candidateRepository = candidateRepository;
            _unitOfWork = unitOfWork;
        }

        #region Acction
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ListpageSize = new List<int>() { 5, 10, 15, 20, 50, 100 };
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            var result = this.GetData(keyword, page, pageSize);

            return Json(new
            {
                data = result.Items,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Details(int id)
        {
            var candidate = _candidateRepository.GetSingleById(id);

            if (candidate == null)
            {
                return Json(new
                {
                    data = candidate,
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                data = candidate,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertOrUpdate(int? id)
        {
            if (id != null)
            {
                var candidate = _candidateRepository.GetSingleById(id.Value);
                candidate.Password = "";
                return View(candidate);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertOrUpdate(Candidate candidate)
        {
            var idCandi = candidate.Id;

            #region Validate data input
            var checkEmail = _candidateRepository.CheckContains(c => c.Email.Equals(candidate.Email));
            var checkPhone = _candidateRepository.CheckContains(c => c.Phone.Equals(candidate.Phone));
            var checkUserName = _candidateRepository.CheckContains(c => c.UserName.Equals(candidate.UserName));

            if (idCandi == 0)
            {
                if (string.IsNullOrEmpty(candidate.Password))
                    ModelState.AddModelError("Password", "This field is required");

                if (checkEmail)
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (checkPhone && candidate.Phone != null)
                    ModelState.AddModelError("Phone", "Phone already exists !!!");
                if (checkUserName)
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }
            else
            {
                var candi = _candidateRepository.GetSingleById(idCandi);

                if (candi == null)
                    return HttpNotFound();

                if (checkEmail && !candi.Email.Equals(candidate.Email))
                    ModelState.AddModelError("Email", "Email already exists !!!");
                if (candi.Phone != null)
                {
                    if (checkPhone)
                        ModelState.AddModelError("Phone", "Phone already exists !!!");
                }
                if (checkUserName && !candi.UserName.Equals(candidate.UserName))
                    ModelState.AddModelError("UserName", "UserName already exists !!!");
            }

            if (!ModelState.IsValid)
            {
                return View(candidate);
            }
            #endregion

            var result = SaveInserOrUpdate(candidate);

            if (result == true)
            {
                if (idCandi == 0)
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
                if (idCandi == 0)
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
 
        [HttpPost]
        public JsonResult Locked(int id)
        {
            var title = "Notification";
            var cadi = _candidateRepository.GetSingleById(id);

            if (cadi == null)
            {
                return Json(new
                {
                    message = "Delete Error !!!",
                    status = false,
                    title
                });
            }
            cadi.Deleted = !cadi.Deleted;
            _unitOfWork.Commit();

            return Json(new
            {
                message = "Delete Successfull !!!",
                status = true,
                title
            });
        }
        #endregion

        #region Method
        private bool SaveInserOrUpdate(Candidate candidate)
        {
            #region Processing Image
            if (string.IsNullOrEmpty(candidate.Image))
            {
                candidate.Image = "/Content/default-avatar.jpg";
            }
            else
            {
                string hostUrl = Request.Url.Scheme + "://" + Request.Url.Host;
                if (!candidate.Image.Contains(hostUrl))
                    candidate.Image = hostUrl + candidate.Image;
                candidate.Image = candidate.Image.CutHostAndSchemePathFile();
            }
            #endregion

            if (candidate.Id == 0)
            {
                candidate.Password = candidate.Password.ToMD5();
                candidate.Status = false;
                candidate.CreatedDate = DateTime.Now;
                candidate.Deleted = false;
                _candidateRepository.Add(candidate);
            }
            else
            {
                var cadi = _candidateRepository.GetSingleById(candidate.Id);
                if (cadi == null)
                {
                    return false;
                }
                cadi.UpdatedDate = DateTime.Now;
                cadi.Name = candidate.Name;
                cadi.UserName = candidate.UserName;
                cadi.Email = candidate.Email;
                cadi.Phone = candidate.Phone;
                cadi.Address = candidate.Address;
                cadi.Education = candidate.Education;
                cadi.WorkExperience = candidate.WorkExperience;
                cadi.Birthday = candidate.Birthday;
                cadi.Sex = candidate.Sex;
                cadi.Image = candidate.Image;
                if (candidate.Password != null)
                {
                    cadi.Password = candidate.Password.ToMD5();
                }
                _candidateRepository.Update(cadi);
            }
            _unitOfWork.Commit();
            return true;
        }

        private PagingModel<Candidate> GetData(string keyword, int page, int pageSize)
        {
            List<Candidate> lstCandi = new List<Candidate>();

            if (!string.IsNullOrEmpty(keyword))
            {
                lstCandi = _candidateRepository.Get(
                    filter: c => c.Deleted == false && (c.UserName.ToLower().Contains(keyword.ToLower())
                    || c.Name.ToLower().Contains(keyword.ToLower()) || c.Email.ToLower().Contains(keyword.ToLower())),
                    orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }
            else
            {
                lstCandi = _candidateRepository.Get(filter: c => c.Deleted == false, orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }

            int totalRow = lstCandi.Count();

            var data = lstCandi.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Candidate>() { TotalRow = totalRow, Items = data };
        }
        #endregion
    }
}
