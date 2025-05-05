using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ceilufas.Models
{
    public class State
    {
        [Key]
        [MaxLength(10)]
        public string Id { get; set; }

        [MaxLength(250)]
        public string Name { get; set; } = "";

        [MaxLength(250)]
        public string NameAr { get; set; } = "";

        public ICollection<Municipality>? Municipalities { get; set; }
    }
}
