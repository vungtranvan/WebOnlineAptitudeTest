using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Model.HisToryTests
{
    public class HisToryTestInsertOrUpdateModel
    {
        /// <summary>
        /// 0: insert | 1: update
        /// </summary>
        public int? TypeAction { get; set; }
        public int CandidateId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int TimeTest { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime TestStartSchedule { get; set; }

        public DateTime TestEndSchedule { get; set; }

    }
}