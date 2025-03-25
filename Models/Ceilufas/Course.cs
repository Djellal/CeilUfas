using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeilUfas.Models.ceilufas
{
    [Table("Courses", Schema = "public")]
    public partial class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NameAr { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public int CourseTypeId { get; set; }

        public CourseType CourseType { get; set; }

        public bool IsActive { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; }

        public ICollection<CourseLevel> CourseLevels { get; set; }
    }
}