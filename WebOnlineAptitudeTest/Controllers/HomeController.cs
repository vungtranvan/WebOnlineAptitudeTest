using Newtonsoft.Json;
using System;
using System.Collections;
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
        private readonly IHistoryTestDetailRepository _historyTestDetailRepository;

        private readonly IUnitOfWork _unitOfWork;


        public HomeController(IQuestionRepository questionRepository,
            ICategoryExamRepository categoryExamRepository,
            IAnswerRepository answerRepository,
            IHistoryTestRepository historyTestRepository,
            IHistoryTestDetailRepository historyTestDetailRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _categoryExamRepository = categoryExamRepository;
            _answerRepository = answerRepository;
            _historyTestRepository = historyTestRepository;
            _historyTestDetailRepository = historyTestDetailRepository;
            _unitOfWork = unitOfWork;

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConfirmTest(List<ResultQuest> resultQuest, int historyTestId)
        {
            try
            {
                var historyTest = this._historyTestRepository.GetSingleById(historyTestId);
                var categoryId = historyTest.CategoryExamId;
                var candidateId = historyTest.CandidateId;
          

                var checktime = (int)(historyTest.CategoryExam.TimeTest * 60) - (int)(DateTime.Now - historyTest.DateStartTest.Value).TotalSeconds + 30;

                if (checktime >= 0 && historyTest.DateEndTest == null)
                {
                    historyTest.DateEndTest = DateTime.Now;
                    this._historyTestRepository.Update(historyTest);



                    foreach (var item in resultQuest)
                    {
                        var itemResult = item.Result == null ? "" : item.Result;
                        HistoryTestDetail historyTestDetail = new HistoryTestDetail();
                        historyTestDetail.QuestionId = item.QuestionId;
                        historyTestDetail.AnswerChoice = itemResult;
                        historyTestDetail.HistoryTestId = historyTestId;

                        Question question = this._questionRepository.GetSingleById(item.QuestionId);
                        List<string> answerResult = new List<string>();

                        foreach (var answer in question.Answers)
                        {
                            if (answer.Correct == true)
                            {
                                answerResult.Add(answer.AnswerInQuestion.Value.ToString());
                            }
                        }

                        if (itemResult != null)
                        {
                            List<string> resultSubmit = itemResult.Split(',').ToList();

                            var checkTrue = resultSubmit.All(answerResult.Contains) && resultSubmit.Count == answerResult.Count;
                            if (checkTrue == true)
                            {
                                historyTestDetail.Mark = question.Mark;
                            }
                            else
                            {
                                historyTestDetail.Mark = 0;
                            }
                        }
                        else
                        {
                            historyTestDetail.Mark = 0;
                        }

                        this._historyTestDetailRepository.Add(historyTestDetail);

                    }
                    this._unitOfWork.Commit();
                }
                return Json(new { data = "", Status = "Success" }, JsonRequestBehavior.AllowGet);


            }
            catch (NullReferenceException e)
            {

            return Json(new {data = e.Message, Status = "false"}, JsonRequestBehavior.AllowGet);
              
            }


        }


        [HttpPost]
        public ActionResult CheckQuest(int CandidateId)
        {

            var historyTest = this._historyTestRepository.Get(x => x.CandidateId == CandidateId).OrderBy(x => x.CategoryExamId);
            var historyTestId = 0;
            var lastSeconds = 0;
            var data = "";
            var categoryId = 0;
            var categoryName = "";
            

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
                        lastSeconds = (int)(item.CategoryExam.TimeTest * 60) - (int)(DateTime.Now - DateStartTest.Value).TotalSeconds;
                        if (lastSeconds <= 0 || item.DateEndTest != null)
                        {
                            continue;
                        }else
                        {
                            data = "Continue";
                            categoryId = item.CategoryExamId;
                            historyTestId = item.Id;
                            categoryName = item.CategoryExam.Name;
                            break;
                        }                  
                    }
                    else
                    {
                        historyTestId = item.Id;
                        data = "NotStart";
                        categoryId = item.CategoryExamId;
                        categoryName = item.CategoryExam.Name;
                        break;
                    }
                }
                if (categoryId == 0)
                {
                    data = "AllOver";
                }

                return Json(new { data = data,
                        Status = "Success",
                        currentCategoryId = categoryId,
                        currentCategoryName = categoryName,
                        historyTestId = historyTestId,
                        timeSecond = lastSeconds
                }, JsonRequestBehavior.AllowGet);
                
            }
        }
        public ActionResult StartQuest(int historyTestId)
        {

            try
            {
                var historyTest = this._historyTestRepository.GetSingleById(historyTestId);
                if (historyTest.DateStartTest == null)
                {
                    historyTest.DateStartTest = DateTime.Now;
                    this._historyTestRepository.Update(historyTest);
                    this._unitOfWork.Commit();
                }
              
                return Json(new { data = "createSuccess", Status = "Success", currentCategoryId = 0, timeSecond = 0 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return Json(new { data = "createFalse", Status = "False", currentCategoryId = 0, timeSecond = 0 }, JsonRequestBehavior.AllowGet);

            }


        }
        public ActionResult CreateQuest(int CategoryExamId)
        {

            var result = this._questionRepository.Get(x => x.CategoryExamId == CategoryExamId);
            var data = JsonConvert.SerializeObject(result, Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });


            return Json(new {data, Status = "Success" }, JsonRequestBehavior.AllowGet);


        }
    }

}