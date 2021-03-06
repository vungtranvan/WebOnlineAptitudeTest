namespace WebOnlineAptitudeTest.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CategoryExam")]
    public partial class CategoryExam
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoryExam()
        {
            HistoryTests = new HashSet<HistoryTest>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }

        [Display(Name = "Choise Category")]
        public string Name { get; set; }

        public int TimeTest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryTest> HistoryTests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
   
        public virtual ICollection<Question> Questions { get; set; }
    }
}
