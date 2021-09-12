﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Models.ViewModels
{
    public class ResultQuest
    {
        public int id { get; set; }
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public ICollection<ResultAnswer> resultAnswers { get; set; }
        public string Correct { get; set; }
    }
}