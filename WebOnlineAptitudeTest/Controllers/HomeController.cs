using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Enums;
using WebOnlineAptitudeTest.Models;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;
using WebOnlineAptitudeTest.Models.ViewModels;


namespace WebOnlineAptitudeTest.Controllers
{
    [FrontEndAuthorize]
    public class HomeController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ICategoryExamRepository _categoryExamRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IHistoryTestRepository _historyTestRepository;
        private readonly IHistoryTestDetailRepository _historyTestDetailRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly ITestScheduleRepository _testScheduleRepository;
        private readonly ITransferRepository _transferRepository;

        private readonly IUnitOfWork _unitOfWork;


        public HomeController(IQuestionRepository questionRepository,
            ICategoryExamRepository categoryExamRepository,
            IAnswerRepository answerRepository,
            IHistoryTestRepository historyTestRepository,
            IHistoryTestDetailRepository historyTestDetailRepository,
            ICandidateRepository candidateRepository,
            ITestScheduleRepository testScheduleRepository,
            ITransferRepository transferRepository,
            IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _categoryExamRepository = categoryExamRepository;
            _answerRepository = answerRepository;
            _historyTestRepository = historyTestRepository;
            _historyTestDetailRepository = historyTestDetailRepository;
            _candidateRepository = candidateRepository;
            _testScheduleRepository = testScheduleRepository;
            _transferRepository = transferRepository;
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
                double totalMark = 0;
                double resultCorectMark = 0;
                var checktime = (int)(historyTest.CategoryExam.TimeTest * 60) - (int)(DateTime.Now - historyTest.DateStartTest.Value).TotalSeconds + 30;
                var testSchedule = this._testScheduleRepository.GetSingleById(historyTest.TestScheduleId);

                if (checktime >= 0 && historyTest.DateEndTest == null)
                {
                    historyTest.DateEndTest = DateTime.Now;
                    historyTest.Status = EnumStatusHistoryTest.Done;
                    List<Question> questions = new List<Question>();
                    foreach (var q in resultQuest)
                    {
                        Question quest = this._questionRepository.GetSingleById(q.QuestionId);
                        questions.Add(quest);
                    }
                    foreach (var item in resultQuest)
                    {
                        var itemResult = item.Result == null ? "" : item.Result;

                        HistoryTestDetail historyTestDetail = this._historyTestDetailRepository
                            .Get(x => x.QuestionId == item.QuestionId && x.HistoryTestId == historyTestId).FirstOrDefault();
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

                        this._historyTestDetailRepository.Update(historyTestDetail);

                    }
                    resultCorectMark = this._historyTestDetailRepository.Get(x => x.HistoryTestId == historyTestId).Select(y => Convert.ToDouble(y.Mark)).Sum();
                    totalMark = questions.Select(x => x.Mark).Sum();
                    historyTest.CorectMark = Math.Round(resultCorectMark, 2, MidpointRounding.ToEven);
                    historyTest.TotalMark = Math.Round(totalMark, 2, MidpointRounding.ToEven);
                    historyTest.PercentMark = Math.Round((resultCorectMark / totalMark) * 100, 2, MidpointRounding.ToEven);
                    historyTest.TimeTest = Math.Round((historyTest.DateEndTest != null ? historyTest.DateEndTest.Value - historyTest.DateStartTest.Value : new TimeSpan()).TotalSeconds, 0, MidpointRounding.ToEven);
                    this._historyTestRepository.Update(historyTest);       

                    if (historyTest.CategoryExamId == 3)
                    {

                        double averageMark = this._historyTestRepository
                            .Get(x => x.CandidateId == historyTest.CandidateId && x.TestScheduleId == historyTest.TestScheduleId)
                            .Sum(y =>y.PercentMark.Value) / 3;

                        if (averageMark >= 80)
                        {
                            Transfer transfer = new Transfer();
                            transfer.CandidateId = historyTest.CandidateId;
                            this._transferRepository.Add(transfer);
                        }                  
                    }

                    this._unitOfWork.Commit();
                }


                return Json(new
                {
                    data = "",
                    categoryName = historyTest.CategoryExam.Name,
                    CorectMark = resultCorectMark,
                    TotalMark = totalMark,
                    PercentMark = Math.Round((resultCorectMark / totalMark) * 100, 2, MidpointRounding.ToEven),
                    TotalTime = historyTest.DateEndTest == null ? 
                    Math.Round((DateTime.Now - historyTest.DateStartTest.Value).TotalSeconds, 0, MidpointRounding.ToEven): 
                    Math.Round((historyTest.DateEndTest.Value - historyTest.DateStartTest.Value).TotalSeconds, 0, MidpointRounding.ToEven),
                    Status = "Success"
                }, JsonRequestBehavior.AllowGet);

            }
            catch (NullReferenceException e)
            {

                return Json(new { data = e.Message, Status = "false" }, JsonRequestBehavior.AllowGet);

            }


        }


        [HttpPost]
        public ActionResult CheckQuest(int CandidateId)
        {
            var historyTest = this._historyTestRepository.Get(x => x.CandidateId == CandidateId && x.Deleted == false).OrderBy(x => x.CategoryExamId);
            var historyTestId = 0;
            var lastSeconds = 0;
            var status = "";
            var categoryId = 0;
            var categoryName = "";
            var testSchedule = this._testScheduleRepository.GetSingleById(historyTest.FirstOrDefault().TestScheduleId);


            if (historyTest.Count() <= 0)
            {
                return Json(new { Status = "NotInSchedule", currentCategoryId = 0, timeSecond = 0 }, JsonRequestBehavior.AllowGet);

            }
            else if (historyTest.FirstOrDefault().TestSchedule.DateStart > DateTime.Now)
            {
                return Json(new
                {
                    testScheduleStartValue = historyTest.FirstOrDefault().TestSchedule.DateStart,
                    testScheduleEndValue = historyTest.FirstOrDefault().TestSchedule.DateEnd,
                    Status = "NotStartSchedule",
                    currentCategoryId = 0,
                    timeSecond = 0
                }, JsonRequestBehavior.AllowGet);
            }
            else if (historyTest.FirstOrDefault().TestSchedule.DateEnd < DateTime.Now)
            {
                var datahistoryTest = historyTest.Select(x => new
                {
                    CorectMark = x.CorectMark,
                    DateStartTest = x.DateStartTest.ToString(),
                    DateEndTest = x.DateEndTest.ToString(),
                    CategoryExam = x.CategoryExam.Name,
                    TotalMark = x.HistoryTestDetails.Sum(q => q.Question.Mark),
                    PercentMark = x.PercentMark,
                    TotalTime = Math.Round((x.DateEndTest != null ? x.DateEndTest.Value - x.DateStartTest.Value : new TimeSpan()).TotalSeconds, 0, MidpointRounding.ToEven),

                });

                var res = JsonConvert.SerializeObject(datahistoryTest.AsEnumerable());

                return Json(new
                {
                    data = res,
                    testScheduleStartValue = historyTest.FirstOrDefault().TestSchedule.DateStart,
                    testScheduleEndValue = historyTest.FirstOrDefault().TestSchedule.DateEnd,
                    Status = "EndSchedule",
                    currentCategoryId = 0,
                    timeSecond = 0
                }, JsonRequestBehavior.AllowGet);
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
                        }
                        else
                        {
                            status = "Continue";
                            categoryId = item.CategoryExamId;
                            historyTestId = item.Id;
                            categoryName = item.CategoryExam.Name;
                            break;
                        }
                    }
                    else
                    {
                        historyTestId = item.Id;
                        status = "NotStart";
                        categoryId = item.CategoryExamId;
                        categoryName = item.CategoryExam.Name;
                        if (item.CategoryExamId == 1)
                        {
                            historyTestId = item.Id;
                            status = "Beginer";
                            categoryId = item.CategoryExamId;
                            categoryName = item.CategoryExam.Name;
                            break;
                        }

                        break;
                    }
                }

                if (categoryId == 0)
                {
                    Candidate candidate = this._candidateRepository.GetSingleById(CandidateId);
                    bool checkDone = true;
                    foreach (var ht in historyTest)
                    {
                        if (ht.DateEndTest == null)
                        {
                            checkDone = false;
                        }
                    }
                    if (checkDone == true)
                    {
                        candidate.Status = EnumStatusCandidate.Done;
                    }
                    else
                    {
                        candidate.Status = EnumStatusCandidate.Undone;
                    }

                    this._candidateRepository.Update(candidate);
                    this._unitOfWork.Commit();
                    status = "AllOver";

                    var datahistoryTest = historyTest.Select(x => new
                    {
                        CorectMark = x.CorectMark,
                        DateStartTest = x.DateStartTest.ToString(),
                        DateEndTest = x.DateEndTest.ToString(),
                        CategoryExam = x.CategoryExam.Name,
                        TotalMark = x.HistoryTestDetails.Sum(q => q.Question.Mark),
                        PercentMark = x.PercentMark,
                        TotalTime = Math.Round((x.DateEndTest != null ? x.DateEndTest.Value - x.DateStartTest.Value : new TimeSpan()).TotalSeconds, 0, MidpointRounding.ToEven),

                    });

                    var res = JsonConvert.SerializeObject(datahistoryTest.AsEnumerable());

                    return Json(new
                    {
                        data = res,
                        Status = status,
                        testScheduleEndValue = testSchedule.DateEnd,
                        testScheduleStartValue = testSchedule.DateStart
                    }, JsonRequestBehavior.AllowGet); ;
                }

                return Json(new
                {
                    data = "",
                    Status = status,
                    currentCategoryId = categoryId,
                    currentCategoryName = categoryName,
                    historyTestId = historyTestId,
                    timeSecond = lastSeconds,
                    testScheduleStart = testSchedule.DateStart.ToString(),
                    testScheduleEnd = testSchedule.DateEnd.ToString(),
                    testScheduleEndValue = testSchedule.DateEnd,
                    testScheduleStartValue = testSchedule.DateStart
                }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult StartQuest(int historyTestId)
        {

            try
            {
                var historyTest = this._historyTestRepository.Get(x => x.Id == historyTestId && x.Deleted == false).FirstOrDefault();
                if (historyTest.DateStartTest == null)
                {
                    historyTest.DateStartTest = DateTime.Now;
                    historyTest.Status = EnumStatusHistoryTest.InProgress;
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
        [HttpPost]
        public ActionResult CreateQuest(int historyTestId)
        {
            var historyTest = this._historyTestRepository.GetSingleById(historyTestId);

            var CategoryExamId = historyTest.CategoryExamId;
            var checkExist = this._historyTestDetailRepository.Get(x => x.HistoryTestId == historyTestId);

            List<Question> result = new List<Question>();
            List<Question> questions = this._questionRepository.Get(x => x.CategoryExamId == CategoryExamId && x.Deleted != true && x.Status != false).ToList();

            if (checkExist.Count() <= 0)
            {
                var resultId = questions.OrderBy(x => Guid.NewGuid()).Take(5).Select(x => x.Id).ToList().OrderBy(x => x.ToString());

                foreach (var id in resultId)
                {
                    Question q = questions.Find(x => x.Id == id);

                    HistoryTestDetail historyTestDetail = new HistoryTestDetail();
                    historyTestDetail.HistoryTestId = historyTestId;
                    historyTestDetail.QuestionId = q.Id;

                    this._historyTestDetailRepository.Add(historyTestDetail);
                    result.Add(q);
                }
            }
            else
            {
                foreach (var q in checkExist)
                {
                    Question quest = questions.Find(x => x.Id == q.QuestionId);
                    result.Add(quest);
                }
            }

            var sendResult = result.Select(x => new
            {
                countCorect = x.Answers.Count(a => a.Correct) > 1 ? x.Answers.Count(a => a.Correct).ToString() : "1",
                Answers = x.Answers.Select(y => new
                {
                    Id = y.Id,
                    QuestionId = y.QuestionId,
                    Name = y.Name,
                    AnswerInQuestion = y.AnswerInQuestion

                }),
                CategoryExamId = x.CategoryExamId,
                CategoryExamName = x.CategoryExamName,
                Id = x.Id,
                Name = x.Name,
                Status = x.Status,
                Mark = x.Mark
            });

            var data = JsonConvert.SerializeObject(sendResult.AsEnumerable(), Formatting.Indented,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            Candidate candidate = this._candidateRepository.GetSingleById(historyTest.CandidateId);
            candidate.Status = EnumStatusCandidate.InProgress;
            this._candidateRepository.Update(candidate);
            this._unitOfWork.Commit();
            return Json(new { data, Status = "Success" }, JsonRequestBehavior.AllowGet);


        }
    }

}