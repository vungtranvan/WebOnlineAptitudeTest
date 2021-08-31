using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Model.TestSchedules
{
    public class TestScheduleViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TimeTest { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? Status { get; set; }
    }
}