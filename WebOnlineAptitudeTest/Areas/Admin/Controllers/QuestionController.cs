using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOnlineAptitudeTest.Models;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;
using WebOnlineAptitudeTest.Models.ViewModels;

namespace WebOnlineAptitudeTest.Areas.Admin.Controllers
{
    [BackEndAuthorize]
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
                    return View("InsertOrUpdate");
                }
                else if (question.Answers.Count < 2)
                {
                    TempData["XMessage"] = new XMessage("Notification", "You need more than 2 answer(s) !!!", EnumCategoryMess.error);
                    this.DropDownCategoryExam();
                    return View("InsertOrUpdate");
                }
            }
            catch (ArgumentNullException)
            {
                this.DropDownCategoryExam();

                TempData["XMessage"] = new XMessage("Notification", "You need answer(s) !!!", EnumCategoryMess.error);
                return View("InsertOrUpdate");
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
                return View("InsertOrUpdate");
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

        public ActionResult UploadFile()
        {
            ViewBag.Category = this._categoryExamRepository.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            ViewBag.Category = this._categoryExamRepository.GetAll();
            try
            {
                if (Path.GetExtension(file.FileName) != ".txt")
                {
                    TempData["XMessage"] = new XMessage("Notification", "You must be upload txt file !!!", EnumCategoryMess.error);
                    return View();
                }
               
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/Content/assets/backend/questUploadFiles"), _FileName);
                    file.SaveAs(_path);

                    if (System.IO.File.Exists(_path))
                    {
                        string[] lines = System.IO.File.ReadAllLines(_path);


                        Question question = new Question();

                        var questions = new List<KeyValuePair<string, List<string>>>();
                        List<List<string>> quests = new List<List<string>>();

                        List<string> quest = new List<string>();

                        string questionContent = "";

                        List<string> alphabet = new List<string>();
                        List<ResultQuestUpload> resultQuests = new List<ResultQuestUpload>();


                        char charA = 'A';
                        alphabet.Add(charA.ToString() + ". ");
                        for (int i = 0; i < 25; i++)
                        {
                            charA = nextChr(charA);
                            alphabet.Add(charA + ". ");
                        }

                        foreach (string line in lines)
                        {
                            quest.Add(line);

                            if (line.Contains("ANSWER:"))
                            {
                                quests.Add(quest);
                                quest = new List<string>();
                            }

                        }

                        foreach (var qu in quests)
                        {
                            ResultQuestUpload resultQuest = new ResultQuestUpload();
                            resultQuest.resultAnswers = new List<KeyValuePair<string, ResultAnswerUpload>>();
                            ResultAnswerUpload resultAnswer = new ResultAnswerUpload();
                            bool questTitle = false;
                            bool firstLine = true;
                            bool haveAnwser = false;
                            foreach (var q in qu)
                            {
                                if ((alphabet.Contains(q.Substring(0, 3)) || q.Substring(0, 5).Contains("MARK:") || q.Substring(0, 7).Contains("ANSWER:")) && questTitle == false)
                                {
                                    resultQuest.Name = questionContent;
                                    questionContent = q;
                                    questTitle = true;
                                    firstLine = false;
                                }
                                else if (alphabet.Contains(q.Substring(0, 3)) && questTitle == true)
                                {
                                    resultAnswer = new ResultAnswerUpload();
                                    resultAnswer.Name = questionContent;
                                    var keyValueResultAnswer = new KeyValuePair<string, ResultAnswerUpload>(questionContent.Substring(0, 1), resultAnswer);
                                    resultQuest.resultAnswers.Add(keyValueResultAnswer);
                                    questionContent = q;
                                    firstLine = false;
                                    haveAnwser = true;
                                }
                                else if (q.Substring(0, 5).Contains("MARK:") && questTitle == true)
                                {
                                    resultAnswer = new ResultAnswerUpload();
                                    resultAnswer.Name = questionContent;
                                    var keyValueResultAnswer = new KeyValuePair<string, ResultAnswerUpload>(questionContent.Substring(0, 1), resultAnswer);
                                    resultQuest.resultAnswers.Add(keyValueResultAnswer);
                                    string mark = "";

                                    if (haveAnwser == true)
                                    {
                                        mark = q.Replace("MARK:", "");
                                    }
                                    else
                                    {
                                        mark = questionContent.Replace("MARK:", "");
                                    }
                                    resultQuest.Mark = mark.Replace(" ", "").Substring(0, 1);
                                    questionContent = "";
                                    firstLine = false;
                                }
                                else if (q.Substring(0, 7).Contains("ANSWER:") && questTitle == true)
                                {
                                    questionContent = "";
                                    string correct = q.Replace("ANSWER:", "");
                                    resultQuest.Result = correct.Replace(" ", "").Length > 0 ? correct.Replace(" ", "").Split(',').ToList() : new List<string>();
                                    firstLine = false;
                                }
                                else if (firstLine == true)
                                {
                                    questionContent += q;
                                    firstLine = false;
                                }
                                else
                                {
                                    questionContent += ("<br/>" + q);
                                }

                                if (q.Substring(0, 5).Contains("MARK:") && questTitle == true && haveAnwser == false)
                                {
                                    string mark = q.Replace("MARK:", "");

                                    resultQuest.Mark = mark.Replace(" ", "").Substring(0, 1);
                                    questionContent = "";
                                    firstLine = false;
                                }

                                if (q.Substring(0, 7).Contains("ANSWER:") && questTitle == true && haveAnwser == false)
                                {
                                    questionContent = "";
                                    string correct = q.Replace("ANSWER:", "");
                                    resultQuest.Result = correct.Replace(" ", "").Length > 0 ? correct.Replace(" ", "").Split(',').ToList() : new List<string>();
                                    firstLine = false;
                                }
                            }
                            resultQuests.Add(resultQuest);
                        }



                        foreach (var q in resultQuests)
                        {
                            foreach (var a in q.resultAnswers)
                            {
                                if (q.Result.Contains(a.Key))
                                {
                                    a.Value.Correct = true;
                                }
                                else
                                {
                                    a.Value.Correct = false;
                                }
                            }
                        }
                        if (resultQuests.Count <= 0)
                        {
                            TempData["XMessage"] = new XMessage("Notification", "Uploaded Faild !!!", EnumCategoryMess.error);
                            return View();
                        }
                        else
                        {

                            TempData["XMessage"] = new XMessage("Notification", "Uploaded Successfully !!!", EnumCategoryMess.success);
                            ViewBag.resultQuests = resultQuests;
                            return View();
                        }


                    }

                }
                else
                {
                    TempData["XMessage"] = new XMessage("Notification", "Uploaded Faild !!!", EnumCategoryMess.error);
                    return View();
                }
                return View();
            }
            catch (Exception)
            {
                TempData["XMessage"] = new XMessage("Notification", "Upload Faild !!!", EnumCategoryMess.error);
                return View();
            }
        }
        [HttpPost]
        public ActionResult SaveUploadFile(List<Question> quest)
        {
            if (!ModelState.IsValid)
            {             
                List<String> errora = new List<string>();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errora.Add(error.ErrorMessage);
                    }
                }
                TempData["XMessage"] = new XMessage("Notification", "errora !!!", EnumCategoryMess.error);
                return View("UploadFile");
            }
            else
            {
                try
                {
                    foreach (var item in quest)
                    {
                        item.CreatedDate = DateTime.Now;
                        this._questionRepository.Add(item);
                    }
 
                    this._unitOfWork.Commit();
                    TempData["XMessage"] = new XMessage("Notification", "Save Success !!!", EnumCategoryMess.success);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    TempData["XMessage"] = new XMessage("Notification", "Save Faild !!!", EnumCategoryMess.error);
                    return View("UploadFile");
                }
            }

          
        }
        private char nextChr(char letter)
        {
            char nextChar;
            if (letter == 'z')
                nextChar = 'a';
            else if (letter == 'Z')
                nextChar = 'A';
            else
                nextChar = (char)(((int)letter) + 1);

            return nextChar;
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