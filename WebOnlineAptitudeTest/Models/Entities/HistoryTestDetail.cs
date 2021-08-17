namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HistoryTestDetail")]
    public partial class HistoryTestDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int HistoryTestId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QuestionId { get; set; }

        [Required]
        [StringLength(50)]
        public string AnswerChoice { get; set; }

        public double Mark { get; set; }

        public virtual HistoryTest HistoryTest { get; set; }

        public virtual Question Question { get; set; }
    }
}
