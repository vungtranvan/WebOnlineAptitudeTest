using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Repositories;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{


    public class QuestionController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly IAnswerRepository _answerRepository;

        public QuestionController(IQuestionRepository questionRepository,
            ICategoryExamRepository categoryExamRepository,
            IAnswerRepository answerRepository)
        {
            _questionRepository = questionRepository;
            _categoryExamRepository = categoryExamRepository;
            _answerRepository = answerRepository;

        }
        // GET: Admin/Question
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult InsertOrUpdate(int? id)
        {

            ViewBag.CategoryId = new SelectList(_categoryExamRepository.GetAll().AsEnumerable(), "CategoryId", "CategoryName");

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

                }
            }

            this.DropDownCategoryExam();
            return RedirectToAction("InsertOrUpdate");

        }

        private void DropDownCategoryExam(int categoryExamId = 0)
        {
            int selectDrop = 0;
            var lstCateEx = _categoryExamRepository.GetAll().ToList();

            if (categoryExamId != 0)
            {
                foreach (var item in lstCateEx)
                {
                    if (item.Id == categoryExamId)
                        selectDrop = lstCateEx.IndexOf(item);
                }
            }
            ViewBag.CategoryExamId = new SelectList(lstCateEx, "Id", "Name", selectDrop);
        }
    }
}