namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transfer")]
    public partial class Transfer
    {
        public int Id { get; set; }

        public int CandidateId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? Deleted { get; set; }

        public virtual Candidate Candidate { get; set; }
    }
}
