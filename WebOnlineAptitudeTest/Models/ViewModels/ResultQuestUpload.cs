using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Models.ViewModels
{
   
    public class ResultQuestUpload
    {
        public int id { get; set; }
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public List<string> Result { get; set; }
        public string Mark { get; set; }
        public ICollection<KeyValuePair<string, ResultAnswerUpload>> resultAnswers { get; set; }

    }

}