using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CategoryExamController : BaseController
    {
        private ICategoryExamRepository _categoryExamRepository;
        private IUnitOfWork _unitOfWork;

        public CategoryExamController(ICategoryExamRepository categoryExamRepository, IUnitOfWork unitOfWork)
        {
            _categoryExamRepository = categoryExamRepository;
            _unitOfWork = unitOfWork;
        }

        // GET: Admin/CategoryExam
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult LoadData()
        {
            var result = _categoryExamRepository.Get();

            return Json(new
            {
                data = result,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var result = _categoryExamRepository.GetSingleById(id);

            return Json(new
            {
                data = result,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Update(CategoryExam categoryExam)
        {
            var message = "Edit Successfull !!!";
            var title = "Notification";
            var status = true;

            var cate = _categoryExamRepository.GetSingleById(categoryExam.Id);
            if (cate == null)
            {
                message = "Edit Error !!!";
                status = false;
            }
            cate.Name = categoryExam.Name;
            _unitOfWork.Commit();

            return Json(new
            {
                message,
                status,
                title
            });
        }
    }
}