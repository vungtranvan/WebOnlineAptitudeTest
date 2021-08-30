using Newtonsoft.Json;
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


    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUnitOfWork _unitOfWork;


        public QuestionController(IQuestionRepository questionRepository,
            ICategoryExamRepository categoryExamRepository,
            IAnswerRepository answerRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _categoryExamRepository = categoryExamRepository;
            _answerRepository = answerRepository;
            _unitOfWork = unitOfWork;

        }
        // GET: Admin/Question
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ListpageSize = new List<int>() { 5, 10, 15, 20, 50, 100 };
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _questionRepository.GetData(keyword, page, pageSize);

            var resultData = JsonConvert.SerializeObject(result.Items, Formatting.Indented,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });

            return Json(new
            {
                data = resultData,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Details(int id)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var question = _questionRepository.GetSingleById(id);

            if (question == null)
            {
                return Json(new
                {
                    data = question,
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                data = question,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsertOrUpdate(int? id)
        {
            if (id != null)
            {
                var question = _questionRepository.GetSingleById(id.Value);
                this.DropDownCategoryExam(question.CategoryExamId);
                return View(question);
            }
            this.DropDownCategoryExam();
            return View();
        }

        [HttpPost]
        public ActionResult InsertOrUpdate(Question question, int? id)
        {
            if (!ModelState.IsValid)
            {
                this.DropDownCategoryExam(question.CategoryExamId);
                List<String> errora = new List<string>();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errora.Add(error.ErrorMessage);
                    }
                }

                var abc = errora;
            }
            else
            {
                question.Id = id != null ? id.Value : 0;

                this._questionRepository.InsertOrUpdate(question);

                if (id != null)
                {

                    this._answerRepository.ChangeAnswer(question.Id, question.Answers);
                    this.DropDownCategoryExam();

                    return RedirectToAction("InsertOrUpdate");

                }else
                {
                    return RedirectToAction("Index");
                }

            }

            this.DropDownCategoryExam();
            return RedirectToAction("Index");


        }

        private void DropDownCategoryExam(int categoryExamId = 0)
        {
            var lstCateEx = _categoryExamRepository.GetAll().ToList();
            ViewBag.NewsItemList = new SelectList(lstCateEx, "Id", "Name", categoryExamId);
        }

        public JsonResult Locked(int id)
        {
            var title = "Notification";
            var cadi = _questionRepository.Locked(id);

            if (!cadi)
            {
                return Json(new
                {
                    message = "Delete Error !!!",
                    status = false,
                    title
                });
            }

            return Json(new
            {
                message = "Delete Successfull !!!",
                status = true,
                title
            });
        }
    }
}