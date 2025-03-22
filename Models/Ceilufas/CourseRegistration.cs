using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CeilUfas.Models.ceilufas
{
    [Table("CourseRegistrations", Schema = "public")]
    public partial class CourseRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string InscriptionCode { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstNameAr { get; set; }

        [Required]
        public string LastNameAr { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string BirthStateId { get; set; }

        public State State { get; set; }

        [Required]
        public int BirthMunicipalityId { get; set; }

        public Municipality Municipality { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Tel { get; set; }

        [Required]
        public int ProfessionId { get; set; }

        public Profession Profession { get; set; }

        [Required]
        public int CourseId { get; set; }

        public Course Course { get; set; }

        [Required]
        public int CourseLevelId { get; set; }

        public CourseLevel CourseLevel { get; set; }

        [Required]
        public int SessionId { get; set; }

        public Session Session { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public string Notes { get; set; }

        [Required]
        public decimal PaidFeeValue { get; set; }

        public bool RegistrationTermsAccepted { get; set; }
    }
}