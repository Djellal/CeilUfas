using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ceilufas.Models
{
    public class CourseLevel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [MaxLength(250)]
        public string Name { get; set; } = "";

        [MaxLength(250)]
        public string NameAr { get; set; } = "";

        public int Duration { get; set; } = 0;

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }


        [ForeignKey("NextLevel")]
        public int? NextLevelId { get; set; }
        public CourseLevel? NextLevel { get; set; }
    }
}
