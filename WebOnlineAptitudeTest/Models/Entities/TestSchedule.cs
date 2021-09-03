namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestSchedule")]
    public partial class TestSchedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TestSchedule()
        {
            HistoryTests = new HashSet<HistoryTest>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        public int TimeTest { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? Status { get; set; }

        public bool? Deleted { get; set; }

        public virtual ICollection<HistoryTest> HistoryTests { get; set; }
    }
}
