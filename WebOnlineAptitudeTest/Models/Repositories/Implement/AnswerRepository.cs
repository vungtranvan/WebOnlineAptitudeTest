using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;
using WebOnlineAptitudeTest.Models.Repositories.Interface;

namespace WebOnlineAptitudeTest.Models.Repositories.Implement
{
    public class AnswerRepository : RepositoryBase<Answer>, IAnswerRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnswerRepository(IDbFactory dbFactory, IUnitOfWork unitOfWork) : base(dbFactory)
        {
            _unitOfWork = unitOfWork;

        }

        public bool ChangeAnswer(int questId, ICollection<Answer> listChanged)
        {

            List<Answer> answerDatas = base.GetMulti(x => x.QuestionId == questId).ToList();
            List<Answer> answerChanged = new List<Answer>();
            if (listChanged.Count() > 0 && listChanged != null)
            {
                answerChanged = listChanged.ToList();
            }else
            {
                return false;
            }

            // remove


            var listToRemove = answerDatas.Where(p => answerChanged.All(p2 => p2.Id != p.Id)).ToList();
            foreach (var aRemove in listToRemove)
            {
                base.Delete(aRemove);
            }


            // update 
            var listToUpdate = (from ansData in answerDatas
                                join ansChange in answerChanged on ansData.Id equals ansChange.Id
                                select ansChange).ToList();

            foreach (var aUpdate in listToUpdate)
            {
                var itemUpdate = base.GetSingleById(aUpdate.Id);
                itemUpdate.Name = aUpdate.Name;
                itemUpdate.Correct = aUpdate.Correct;
                itemUpdate.AnswerInQuestion = aUpdate.AnswerInQuestion;

                base.Update(itemUpdate);
            }


            // add

            var listAdd = answerChanged.Where(a => a.Id == 0).ToList();

            foreach (var aAdd in listAdd)
            {
                aAdd.QuestionId = questId;
                base.Add(aAdd);
            }

            _unitOfWork.Commit();

            return true;
        }
    }
}