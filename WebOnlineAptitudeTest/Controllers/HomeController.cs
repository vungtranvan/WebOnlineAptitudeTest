using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;
using WebOnlineAptitudeTest.Models.ViewModels;


namespace WebOnlineAptitudeTest.Controllers
{
    public class HomeController : BaseFrController
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly IUnitOfWork _unitOfWork;


        public HomeController(IQuestionRepository questionRepository,
            ICategoryExamRepository categoryExamRepository,
            IAnswerRepository answerRepository,
            IHistoryTestRepository historyTestRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _categoryExamRepository = categoryExamRepository;
            _answerRepository = answerRepository;
            _historyTestRepository = historyTestRepository;
            _unitOfWork = unitOfWork;

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConfirmTest(List<ResultQuest> resultQuest, string name)
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
        public ActionResult ApplyQuest(int CandidateId)
        {

            var historyTest = this._historyTestRepository.Get(x => x.CandidateId == CandidateId).OrderBy(x => x.CategoryExamId);
            var historyTestId = 0;
            var lastSeconds = 0;
            var status = "";
            var categoryId = 0;

            if (historyTest.Count() <= 0)
            {
                return Json(new { data = "NotInSchedule", Status = "False", currentCategoryId = 0, timeSecond = 0 }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                foreach (var item in historyTest)
                {
                    var DateStartTest = item.DateStartTest;

                    if (DateStartTest != null)
                    {
                        //lastSeconds = (int) (item.CategoryExam) - (int)(DateTime.Now - DateStartTest.Value).TotalSeconds;
                        status = "continue";
                        categoryId = item.CategoryExamId;
                        break;
                    }
                    else
                    {
                        historyTestId = item.Id;
                        break;
                    }
                }

                if (historyTestId != 0)
                {
                    HistoryTest historyUpdate = this._historyTestRepository.GetSingleById(historyTestId);
                    historyUpdate.DateStartTest = DateTime.Now;
                    this._historyTestRepository.Update(historyUpdate);
                }
                else
                {
                    return Json(new { data = "AllOver", Status = "False", currentCategoryId = 0, timeSecond = 0 }, JsonRequestBehavior.AllowGet);
                }
            }

            //var data = JsonConvert.SerializeObject(0, Formatting.Indented,
            //    new JsonSerializerSettings
            //    {
            //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //    });


            this._unitOfWork.Commit();

            return Json(new {data =  "", Status = "Success", currentCategoryId = 0, timeSecond = 0 }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult CreateQuest(int categoryId)
        {
            var result = this._questionRepository.Get(x => x.CategoryExamId == categoryId);

            var data = JsonConvert.SerializeObject(result, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });



            return Json(new { data = data, Status = "Success", currentCategoryId = 0, timeSecond = 0 }, JsonRequestBehavior.AllowGet);
        }
    }

}