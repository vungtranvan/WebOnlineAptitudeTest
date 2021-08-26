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

        public QuestionController(IQuestionRepository questionRepository,ICategoryExamRepository categoryExamRepository)
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

            ViewBag.CategoryId = new SelectList(_categoryExamRepository.GetAll().AsEnumerable(), "CategoryId", "CategoryName");

            if (id != null)
            {
                var question = _questionRepository.GetSingleById(id.Value);
     
                return View(question);
            }
            return View();
        }

        [HttpPost]
       
        public ActionResult InsertOrUpdate(IEnumerable<Answer> anwser,Question question)
        {

            var q = question.Mark;

            foreach (var item in anwser)
            {
                var abc = item.Name;
            }

            return View();
        }
    }
}