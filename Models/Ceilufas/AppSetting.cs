using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeilUfas.Models.ceilufas
{
    [Table("AppSettings", Schema = "public")]
    public partial class AppSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string WebSite { get; set; }

        [Column("FB")]
        [Required]
        public string Fb { get; set; }

        [Required]
        public string LinkredIn { get; set; }

        [Required]
        public string Logo { get; set; }

        public int? CurrentSessionId { get; set; }

        public Session Session { get; set; }

        public bool IsRegistrationOpened { get; set; }
    }
}