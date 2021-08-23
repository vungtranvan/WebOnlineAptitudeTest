using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.DAL.CategoryExams;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    public class CategoryExamController : BaseController
    {
        private ICategoryExamRepository _categoryExamRepository;
        public CategoryExamController()
        {
            _categoryExamRepository = new CategoryExamRepository();
        }

        public CategoryExamController(ICategoryExamRepository categoryExamRepository)
        {
            _categoryExamRepository = categoryExamRepository;
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
            var result = _categoryExamRepository.Get(id);

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
            var result = _categoryExamRepository.Update(categoryExam);
            if (result == false)
            {
                message = "Edit Error !!!";
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