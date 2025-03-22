using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeilUfas.Models.ceilufas
{
    [Table("Sessions", Schema = "public")]
    public partial class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string SessionCode { get; set; }

        [Required]
        public string SessionName { get; set; }

        [Required]
        public string SessionNameAr { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public ICollection<AppSetting> AppSettings { get; set; }

        public ICollection<CourseRegistration> CourseRegistrations { get; set; }
    }
}