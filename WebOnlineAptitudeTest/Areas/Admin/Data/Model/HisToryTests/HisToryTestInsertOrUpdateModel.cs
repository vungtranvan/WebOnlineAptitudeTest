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
        [Display(Name = "Candidate Name")]
        public int CandidateId { get; set; }

        [Display(Name = "Time To Do A One Subject ")]
        [Required(ErrorMessage = "This field is required")]
        public int TimeTest { get; set; }

        [Display(Name = "Test Start Schedule")]
        [Required(ErrorMessage = "This field is required")]
        public DateTime TestStartSchedule { get; set; }

        [Display(Name = "Test End Schedule")]
        public DateTime? TestEndSchedule { get; set; }

    }
}