namespace WebOnlineAptitudeTest.Models.Entities
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("Answer")]
    public partial class Answer
    {
        public int Id { get; set; }

        public int? QuestionId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Answer")]
        [Required]
        [AllowHtml]
        public string Name { get; set; }

        [JsonIgnore]
        public bool Correct { get; set; }

        public int? AnswerInQuestion { get; set; }

        [JsonIgnore]
        public virtual Question Question { get; set; }
    }
}