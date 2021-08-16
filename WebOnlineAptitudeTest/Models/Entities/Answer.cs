namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Answer")]
    public partial class Answer
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        [Required]
        public string Name { get; set; }

        public bool Result { get; set; }

        public virtual Question Question { get; set; }
    }
}
