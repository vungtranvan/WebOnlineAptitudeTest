using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOnlineAptitudeTest.Models.Entities;
using WebOnlineAptitudeTest.Models.Infrastructure;

namespace WebOnlineAptitudeTest.Models.Repositories.Interface
{
    public interface IAnswerRepository : IRepository<Answer>
    {
        bool ChangeAnswer(int questId, ICollection<Answer> listChanged);
    }
}
