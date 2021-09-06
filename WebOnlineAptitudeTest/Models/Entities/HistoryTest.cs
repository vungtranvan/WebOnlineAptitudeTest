namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using WebOnlineAptitudeTest.Enums;

    [Table("HistoryTest")]
    public partial class HistoryTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HistoryTest()
        {
            HistoryTestDetails = new HashSet<HistoryTestDetail>();
        }

        public int Id { get; set; }

        public int CategoryExamId { get; set; }

        public int CandidateId { get; set; }

        public int TestScheduleId { get; set; }

        public int TimeTest { get; set; }

        public DateTime? DateStartTest { get; set; }

        public DateTime? DateEndTest { get; set; }

        public double? CorectMark { get; set; }

        public double? PercentMark { get; set; }

        public double? TotalMark { get; set; }
        public EnumStatusHistoryTest? Status { get; set; }

        public bool? Deleted { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual CategoryExam CategoryExam { get; set; }

        public virtual TestSchedule TestSchedule { get; set; }

        public virtual ICollection<HistoryTestDetail> HistoryTestDetails { get; set; }
    }
}
