using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Areas.Admin.Data.Model.Pagings;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories
{
    public interface IQuestionRepository2 : IRepository<Question>
    {
        bool InsertOrUpdate(Question question);
        bool Locked(int id);
        PagingModel<Question> GetData(string keyword, int page, int pageSize);
    }

    public class QuestionRepository2 : RepositoryBase<Question>, IQuestionRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestionRepository2(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
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
                _unitOfWork.Commit();

            }
            else
            {
                var cadi = base.GetSingleById(question.Id);

                var existingParent = base.DbContext.Questions
                      .Where(p => p.Id == question.Id)
                      .Include(p => p.Answers)
                      .SingleOrDefault();


                if (existingParent != null)
                {
                    // Update parent
                    base.DbContext.Entry(existingParent).CurrentValues.SetValues(question);

                    // Delete children
                    if (question.Answers != null)
                    {
                        foreach (var existingChild in existingParent.Answers.ToList())
                        {
                            if (!question.Answers.Any(c => c.Id == existingChild.Id))
                                base.DbContext.Answers.Remove(existingChild);
                        }
                        // Update and Insert children
                        foreach (var childModel in question.Answers)
                        {
                            var existingChild = existingParent.Answers
                                .Where(c => c.Id == childModel.Id && c.Id != default(int))
                                .SingleOrDefault();

                            if (existingChild != null)
                                // Update child
                                base.DbContext.Entry(existingChild).CurrentValues.SetValues(childModel);
                            else
                            {
                                // Insert child
                                var newChild = new Answer
                                {
                                    Id = childModel.Id,
                                    QuestionId = childModel.QuestionId,
                                    Name = childModel.Name,
                                    Correct = childModel.Correct,
                                    AnswerInQuestion = childModel.AnswerInQuestion

                                    //...
                                };
                                existingParent.Answers.Add(newChild);
                            }
                        }
                    }

                    base.DbContext.SaveChanges();
                }
            }
            return true;
        }

        public bool Locked(int id)
        {
            throw new NotImplementedException();
        }
    }
}