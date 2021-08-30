using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface IQuestionRepository : IRepository<Question>
    {
        bool InsertOrUpdate(Question question);
        bool Locked(int id);
        PagingModel<Question> GetData(string keyword, int page, int pageSize);
    }

    public class QuestionRepository : RepositoryBase<Question>, IQuestionRepository
    {
        private readonly IUnitOfWork _unitOfWork;
       

        public QuestionRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;

        }

        public PagingModel<Question> GetData(string keyword, int page, int pageSize)
        {
            List<Question> lstCandi = base.DbContext.Questions.Include(a => a.Answers).Include(a => a.CategoryExam)
            .Select(q => new {
            Id = q.Id,
            Name = q.Name,
            Status = q.Status,
            Deleted = q.Deleted,
            Mark = q.Mark,
            Answers = q.Answers,
            CategoryExamId = q.CategoryExamId,
            CategoryExamName = q.CategoryExam.Name,
            CreatedDate = q.CreatedDate,
            UpdatedDate = q.UpdatedDate
            }).ToList().Select(b=> new Question {
                Id = b.Id,
                Name = b.Name,
                Status = b.Status,
                Deleted = b.Deleted,
                Mark = b.Mark,
                Answers = b.Answers,
                CategoryExamId = b.CategoryExamId,
                CategoryExamName = b.CategoryExamName,
                CreatedDate = b.CreatedDate,
                UpdatedDate = b.UpdatedDate
            } ).ToList();



            if (!string.IsNullOrEmpty(keyword))
            {
                lstCandi = base.Get(
                    filter: c => c.Deleted == false && (c.Name.ToLower().Contains(keyword.ToLower())
                    || c.Name.ToLower().Contains(keyword.ToLower()) || c.Status.ToString().ToLower().Contains(keyword.ToLower())),
                    orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }
            else
            {
                lstCandi = base.Get(filter: c => c.Deleted == false || c.Deleted == null, orderBy: c => c.OrderByDescending(x => x.Id)).ToList();
            }

            int totalRow = lstCandi.Count();

            var data = lstCandi.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagingModel<Question>() { TotalRow = totalRow, Items = data };
        }

        public bool InsertOrUpdate(Question question)
        {
            if (question.Id == 0)
            {
                base.Add(question);

            }
            else
            {
                Question q = base.GetSingleById(question.Id);

                if (q!= null)
                {
                    q.Name = question.Name;

                    q.CategoryExamId = question.CategoryExamId;
                    q.Mark = question.Mark;
                    q.Status = question.Status;
                    q.Deleted = question.Deleted;


                    base.Update(q);

                }

            }
            _unitOfWork.Commit();

            return true;
        }

        public bool Locked(int id)
        {
            throw new NotImplementedException();
        }
    }
}