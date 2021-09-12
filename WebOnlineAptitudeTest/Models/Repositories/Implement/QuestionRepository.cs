using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        private readonly IUnitOfWork _unitOfWork;


        public QuestionRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;

        }

        public PagingModel<Question> GetData(string keyword, int idCate, int page, int pageSize)
        {

            List<Question> lstQuestion = new List<Question>();
            if (idCate > 0)
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    lstQuestion = base.GetMulti(x => x.Deleted == false && x.CategoryExam.Id.Equals(idCate) && (x.Name.Contains(keyword)
                       || x.CategoryExam.Name.Contains(keyword)), new string[] { "CategoryExam", "Answers" })
                    .Select(b => new Question
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Status = b.Status,
                        Deleted = b.Deleted,
                        Mark = b.Mark,
                        Answers = b.Answers,
                        CategoryExamId = b.CategoryExamId,
                        CategoryExamName = b.CategoryExam.Name,
                        CreatedDate = b.CreatedDate,
                        UpdatedDate = b.UpdatedDate
                    }).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    lstQuestion = base.GetMulti(x => x.Deleted == false && x.CategoryExam.Id.Equals(idCate), new string[] { "CategoryExam", "Answers" })
                    .Select(b => new Question
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Status = b.Status,
                        Deleted = b.Deleted,
                        Mark = b.Mark,
                        Answers = b.Answers,
                        CategoryExamId = b.CategoryExamId,
                        CategoryExamName = b.CategoryExam.Name,
                        CreatedDate = b.CreatedDate,
                        UpdatedDate = b.UpdatedDate
                    }).OrderByDescending(x => x.Id).ToList();
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    lstQuestion = base.GetMulti(x => (x.Deleted == false) && (x.Name.Contains(keyword)
                       || x.CategoryExam.Name.Contains(keyword)), new string[] { "CategoryExam", "Answers" })
                    .Select(b => new Question
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Status = b.Status,
                        Deleted = b.Deleted,
                        Mark = b.Mark,
                        Answers = b.Answers,
                        CategoryExamId = b.CategoryExamId,
                        CategoryExamName = b.CategoryExam.Name,
                        CreatedDate = b.CreatedDate,
                        UpdatedDate = b.UpdatedDate
                    }).OrderByDescending(x => x.Id).ToList();
                }
                else
                {
                    lstQuestion = base.GetMulti(x => x.Deleted == false, new string[] { "CategoryExam", "Answers" })
                    .Select(b => new Question
                    {
                        Id = b.Id,
                        Name = b.Name,
                        Status = b.Status,
                        Deleted = b.Deleted,
                        Mark = b.Mark,
                        Answers = b.Answers,
                        CategoryExamId = b.CategoryExamId,
                        CategoryExamName = b.CategoryExam.Name,
                        CreatedDate = b.CreatedDate,
                        UpdatedDate = b.UpdatedDate
                    }).OrderByDescending(x => x.Id).ToList();
                }
            }
           

            int totalRow = lstQuestion.Count();

            var data = lstQuestion.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Question>() { TotalRow = totalRow, Items = data };
        }

        public bool InsertOrUpdate(Question question)
        {
            if (question.Id == 0)
            {
                question.CreatedDate = DateTime.Now;
                question.Deleted = false;
                base.Add(question);

            }
            else
            {
                Question q = base.GetSingleById(question.Id);

                if (q != null)
                {
                    q.Name = question.Name;
                    q.UpdatedDate = DateTime.Now;
                    q.CategoryExamId = question.CategoryExamId;
                    q.Mark = question.Mark;
                    q.Status = question.Status != null ? true : false;

                    base.Update(q);

                }

            }
            _unitOfWork.Commit();

            return true;
        }

        public bool Locked(int id)
        {
            var quest = base.GetSingleById(id);

            if (quest == null)
            {
                return false;
            }
            quest.Deleted = true;
            base.Update(quest);
            _unitOfWork.Commit();
            return true;
        }
        public List<Question> GetQuestion(int CategoryExamId)
        {
            List<Question> lstQuestion = new List<Question>();

            lstQuestion = base.GetMulti(x => (x.Deleted == false || x.Deleted == null) && x.CategoryExamId == CategoryExamId, new string[] { "CategoryExam", "Answers" })
            .Select(b => new Question
            {
                Id = b.Id,
                Name = b.Name,
                Status = b.Status,
                Deleted = b.Deleted,
                Mark = b.Mark,
                Answers = b.Answers,
                CategoryExamId = b.CategoryExamId,
                CategoryExamName = b.CategoryExam.Name
            }).OrderByDescending(x => x.Id).ToList();

            return lstQuestion;
        }
    }
}