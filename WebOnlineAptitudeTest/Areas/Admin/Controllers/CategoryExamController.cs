using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Areas.Admin.Data.DAL.CategoryExams;

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

        public JsonResult LoadData()
        {
            var result = _categoryExamRepository.Get();

            return Json(new
            {
                data = result,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}