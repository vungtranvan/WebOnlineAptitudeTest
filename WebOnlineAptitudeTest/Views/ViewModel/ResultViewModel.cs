using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Views.ViewModel
{
    public class ResultViewModel
    {
        public int CandidateId { get; set; }
        public List<ResultQuest> ResultQuest { get; set; }

    }

    public class ResultQuest
    {
        public int QuestionId { get; set; }

        public string Answers { get; set; }

    }


}