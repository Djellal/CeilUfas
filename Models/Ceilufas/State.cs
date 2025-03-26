using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeilUfas.Models.ceilufas
{
    [Table("States", Schema = "public")]
    public partial class State
    {
        [Key]
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NameAr { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; }

        public ICollection<Municipality> Municipalities { get; set; }
    }
}