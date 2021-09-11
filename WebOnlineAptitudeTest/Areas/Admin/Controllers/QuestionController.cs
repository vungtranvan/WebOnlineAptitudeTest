using Newtonsoft.Json;
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
    public class QuestionController : BaseController
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
            this.DropDownCategoryExam();
            ViewBag.ListpageSize = new List<int>() { 10, 15, 20, 50, 100 };
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string keyword, int idCate, int page, int pageSize = 3)
        {
            _unitOfWork.DbContext.Configuration.ProxyCreationEnabled = false;
            var result = _questionRepository.GetData(keyword, idCate, page, pageSize);

            var resultData = JsonConvert.SerializeObject(result.Items, Formatting.Indented,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });

            var json = Json(new
            {
                data = resultData,
                totalRow = result.TotalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = Int32.MaxValue;
            return json;
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
                ViewBag.Breadcrumb = "Edit";
                ViewBag.Title = "Edit Question";
                return View(question);
            }
            else
            {
                ViewBag.Title = "Add New Question";
                ViewBag.Breadcrumb = "Add New";
                this.DropDownCategoryExam();
                return View();
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult InsertOrUpdate(Question question, int? id)
        {
            try
            {
                if (question.Answers == null)
                {
                    TempData["XMessage"] = new XMessage("Notification", "You need answer(s) !!!", EnumCategoryMess.error);
                    this.DropDownCategoryExam();
                    return RedirectToAction("InsertOrUpdate");
                }
            }
            catch (ArgumentNullException)
            {
                this.DropDownCategoryExam();

                TempData["XMessage"] = new XMessage("Notification", "You need answer(s) !!!", EnumCategoryMess.error);
                return RedirectToAction("InsertOrUpdate");
            }

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

                TempData["XMessage"] = new XMessage("Notification", "errora !!!", EnumCategoryMess.error);
                return RedirectToAction("InsertOrUpdate");
            }
            else
            {
                question.Id = id != null ? id.Value : 0;

                var result = this._questionRepository.InsertOrUpdate(question);

                if (id != null)
                {

                    this._answerRepository.ChangeAnswer(question.Id, question.Answers);
                    this.DropDownCategoryExam();

                    if (result == true)
                    {
                        TempData["XMessage"] = new XMessage("Notification", "Edit Successfull !!!", EnumCategoryMess.success);
                    }
                    else
                    {
                        TempData["XMessage"] = new XMessage("Notification", "Edit Error !!!", EnumCategoryMess.error);
                    }

                }
                else
                {
                    if (result == true)
                    {
                        TempData["XMessage"] = new XMessage("Notification", "Add Successfull !!!", EnumCategoryMess.success);
                    }
                    else
                    {
                        TempData["XMessage"] = new XMessage("Notification", "Add Error !!!", EnumCategoryMess.error);
                    }

                }
                return RedirectToAction("Index");

            }

        }

        private void DropDownCategoryExam(int categoryExamId = 0)
        {
            var lstCateEx = _categoryExamRepository.Get(orderBy: x => x.OrderByDescending(y => y.Id)).ToList();
            ViewBag.NewsItemList = new SelectList(lstCateEx, "Id", "Name", categoryExamId);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
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