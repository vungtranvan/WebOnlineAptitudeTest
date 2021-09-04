namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Candidate")]
    public partial class Candidate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Candidate()
        {
            HistoryTests = new HashSet<HistoryTest>();
            Transfers = new HashSet<Transfer>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "This field max length is 50")]
        public string UserName { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "This field max length is 50")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "This field max length is 50")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email is not formatted")]
        public string Email { get; set; }

        public string Image { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date, ErrorMessage = "Incorrect format dd/MM/yyyy")]
        public DateTime? Birthday { get; set; }

        public string Address { get; set; }

        [StringLength(30, ErrorMessage = "This field max length is 30")]
        public string Phone { get; set; }

        public bool Sex { get; set; }

        public string Education { get; set; }

        public string WorkExperience { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Status { get; set; }

        public bool Deleted { get; set; }
        
        public virtual ICollection<HistoryTest> HistoryTests { get; set; }

        public virtual ICollection<Transfer> Transfers { get; set; }
    }
}
