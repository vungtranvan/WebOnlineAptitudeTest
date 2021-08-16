namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HistoryTest")]
    public partial class HistoryTest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HistoryTest()
        {
            HistoryTestDetails = new HashSet<HistoryTestDetail>();
        }

        public int Id { get; set; }

        public int CandidateId { get; set; }

        public int CategoryExamId { get; set; }

        public double TotalMark { get; set; }

        public DateTime? DateStartTest { get; set; }

        public DateTime? DateEndTest { get; set; }

        public virtual Candidate Candidate { get; set; }

        public virtual CategoryExam CategoryExam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryTestDetail> HistoryTestDetails { get; set; }
    }
}
