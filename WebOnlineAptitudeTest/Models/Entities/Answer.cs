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

        public int? QuestionId { get; set; }

        public string Name { get; set; }

        public bool Correct { get; set; }

        public int? AnswerInQuestion { get; set; }

        public virtual Question Question { get; set; }
    }
}