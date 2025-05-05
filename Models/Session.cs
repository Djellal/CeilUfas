using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ceilufas.Models
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(30)]
        public string SessionCode { get; set; } = "";

        [MaxLength(250)]
        public string SessionName { get; set; } = "";

        [MaxLength(250)]
        public string SessionNameAr { get; set; } = "";

        public DateTime StartDate { get; set; } = DateTime.Now;

        public DateTime EndDate { get; set; }= DateTime.Now.AddMonths(3);
    }
}
