using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CategoryExamController : BaseController
    {
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly IUnitOfWork _unitOfWork;

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
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var data = _categoryExamRepository.Get().ToList();

            var json = Json(new
            {
                data = data,
                status = true
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
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
            _categoryExamRepository.Update(cate);
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