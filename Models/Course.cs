using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ceilufas.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Code { get; set; } = "";

        [MaxLength(250)]
        public string Name { get; set; } = "";

        [MaxLength(250)]
        public string NameAr { get; set; } = "";

        public bool IsActive { get; set; } = true;
       

        public string Image { get; set; } = "";
        
        public CourseType CourseType { get; set; } = CourseType.Language;
    }

    public enum CourseType
    {
        Language = 1,
        Workshop = 2
    }
}