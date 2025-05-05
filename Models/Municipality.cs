using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ceilufas.Models
{
    public class Municipality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = "";

        [MaxLength(250)]
        public string NameAr { get; set; } = "";

        [Required]
        [ForeignKey("State")]
        [MaxLength(10)]
        public string StateId { get; set; } = "";
        
        public State? State { get; set; }
    }
}
