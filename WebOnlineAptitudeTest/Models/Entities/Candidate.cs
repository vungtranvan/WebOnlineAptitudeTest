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

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "This field max length is 50")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "This field max length is 50")]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "This field max length is 50")]
        [EmailAddress(ErrorMessage = "The email address is not valid")]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public string Address { get; set; }

        [StringLength(30, ErrorMessage = "This field max length is 30")]
        public string Phone { get; set; }

        public bool? Sex { get; set; }

        public string Education { get; set; }

        public string WorkExperience { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Status { get; set; }

        public bool Deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryTest> HistoryTests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transfer> Transfers { get; set; }
    }
}
