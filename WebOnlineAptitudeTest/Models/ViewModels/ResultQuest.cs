using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Models.ViewModels
{
    public class ResultQuest
    {
        public int id { get; set; }
        public int QuestionId { get; set; }
        public string Result { get; set; }
    }
}