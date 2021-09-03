using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;
using WebOnlineAptitudeTest.Views.ViewModel;

namespace WebOnlineAptitudeTest.Controllers
{
    public class HomeController : BaseFrController
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(IQuestionRepository questionRepository,
            ICategoryExamRepository categoryExamRepository,
            IAnswerRepository answerRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _categoryExamRepository = categoryExamRepository;
            _answerRepository = answerRepository;
            _unitOfWork = unitOfWork;

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConfirmTest(ResultViewModel resultViewModel)
        {

            //string reQuestQuest = "";

            //var collection  =  testRequest.Get("collection");

            //foreach (var key in testRequest.AllKeys)
            //{
            //    if (key.Contains("collection"))
            //    {
            //        reQuestQuest = testRequest.Get(key);
            //    }
            //}
            //var abc = HttpUtility.ParseQueryString(collection);


            //var value = abc.Get("q2");

            List<string> lstAppendColumn = new List<string>();
            lstAppendColumn.Add("First");
            lstAppendColumn.Add("Second");
            lstAppendColumn.Add("Third");

            string Response = JsonConvert.SerializeObject(lstAppendColumn);


            return Json(new { Response, Status = "Success", PartName = "123" }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateQuest()
        {
            var result = _questionRepository.GetQuestion(1);


            var data = JsonConvert.SerializeObject(result, Formatting.Indented,
               new JsonSerializerSettings
               {
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore
               });

            return Json(new { data, Status = "Success", PartId = 1}, JsonRequestBehavior.AllowGet);
        }
    }
}