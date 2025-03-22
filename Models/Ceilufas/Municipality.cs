using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeilUfas.Models.ceilufas
{
    [Table("Municipalities", Schema = "public")]
    public partial class Municipality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NameAr { get; set; }

        [Required]
        public string StateId { get; set; }

        public State State { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}