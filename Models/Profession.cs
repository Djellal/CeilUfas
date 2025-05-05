using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ceilufas.Models
{
    public class Profession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [Required]
        [MaxLength(300)]
        [Display(Name = "Name (Arabic)")]
        public string NameAr { get; set; } = "";

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Fee Value")]
        public decimal FeeValue { get; set; }
    }
}