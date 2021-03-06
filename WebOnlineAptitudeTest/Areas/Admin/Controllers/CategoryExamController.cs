using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    [BackEndAuthorize]
    public class CategoryExamController : Controller
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
            ViewBag.ListpageSize = new List<int>() { 10, 15, 20, 50, 100 };
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _categoryExamRepository.GetData(keyword, page, pageSize);

            var json = Json(new
            {
                data = result.Items,
                totalRow = result.TotalRow,
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
            if (cate.Name != categoryExam.Name && _categoryExamRepository.CheckContains(x => x.Name.Equals(categoryExam.Name)))
            {
                return Json(new
                {
                    message = "Name already exists !!! !!!",
                    status = false,
                    title
                });
            }

            cate.Name = categoryExam.Name;
            cate.TimeTest = categoryExam.TimeTest;
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