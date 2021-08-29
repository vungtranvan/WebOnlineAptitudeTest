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
            throw new NotImplementedException();
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