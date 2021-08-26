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

        public QuestionController(IQuestionRepository questionRepository, ICategoryExamRepository categoryExamRepository)
        {
            _questionRepository = questionRepository;
            _categoryExamRepository = categoryExamRepository;
        }
        // GET: Admin/Question
        public ActionResult Index()
        {
            return View();
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
        public ActionResult InsertOrUpdate(IEnumerable<Answer> anwser, Question question)
        {
            if (!ModelState.IsValid)
            {
                this.DropDownCategoryExam(question.CategoryExamId);
                return View();
            }
            var q = question.Mark;

            foreach (var item in anwser)
            {
                var abc = item.Name;
            }

            this.DropDownCategoryExam();
            return View();
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