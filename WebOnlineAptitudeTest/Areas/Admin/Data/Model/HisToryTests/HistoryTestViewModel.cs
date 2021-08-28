using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests
{
    public class HistoryTestViewModel
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public DateTime? TestStartSchedule { get; set; }
        public DateTime TestEndSchedule { get; set; }
        public double? TotalMark { get; set; }
        public int? Status { get; set; }
    }
}