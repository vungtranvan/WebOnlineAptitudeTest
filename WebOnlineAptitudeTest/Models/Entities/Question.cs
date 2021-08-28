namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Question()
        {
            HistoryTestDetails = new HashSet<HistoryTestDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(225)]
        [Display(Name="Question")]
        [DataType(DataType.MultilineText)]
        public string Name { get; set; }

        public int CategoryExamId { get; set; }

        [RegularExpression(@"(1|2|3|4|5){1}$",
     ErrorMessage = "only allowed 1 - 5")]
        public double Mark { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? Status { get; set; }

        public bool? Deleted { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Answer> Answers { get; set; }

        public virtual CategoryExam CategoryExam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryTestDetail> HistoryTestDetails { get; set; }
    }
}
