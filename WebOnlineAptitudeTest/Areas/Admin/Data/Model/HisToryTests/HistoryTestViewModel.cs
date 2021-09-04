using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebOnlineAptitudeTest.Models.Entities;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests
{
    public class HistoryTestViewModel
    {
        public int CandidateId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public double ToTalMark { get; set; }
        public bool Status { get; set; }
        public ICollection<HistoryTest> HistoryTests { get; set; }
    }
}