using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebOnlineAptitudeTest.Areas.Admin.Data.Model.TestSchedules
{
    public class TestScheduleInsertOrUpdateRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(500)]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime DateStart { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public DateTime DateEnd { get; set; }

        [Display(Name = "List Candidate Name")]
        [Required(ErrorMessage = "This field is required")]
        public List<int> CandidateId { get; set; }
    }
}