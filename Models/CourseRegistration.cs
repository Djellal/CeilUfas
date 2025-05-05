using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ceilufas.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Ceilufas.Models
{
    public class CourseRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }


        [Required]
        [ForeignKey("User")]        
        public string UserId { get; set; }=string.Empty;
        public ApplicationUser? User { get; set; }
        

        [Required]
        [MaxLength(20)]
        public string InscriptionCode { get; set; } = "";

        [Required]
        [MaxLength(300)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(300)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";

        [Required]
        [MaxLength(300)]
        [Display(Name = "First Name (Arabic)")]
        public string FirstNameAr { get; set; } = "";

        [Required]
        [MaxLength(300)]
        [Display(Name = "Last Name (Arabic)")]
        public string LastNameAr { get; set; } = "";

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [ForeignKey("BirthState")]
        [Display(Name = "Birth State")]
        public string BirthStateId { get; set; } = "";
        public State? BirthState { get; set; }

        [Required]
        [ForeignKey("BirthMunicipality")]
        [Display(Name = "Birth Municipality")]
        public int BirthMunicipalityId { get; set; }
        public Municipality? BirthMunicipality { get; set; }

        [Required]
        [MaxLength(300)]
        public string Address { get; set; } = "";

        [Required]
        [MaxLength(100)]
        [Phone]
        public string Tel { get; set; } = "";

        [Required]
        [ForeignKey("Profession")]
        [Display(Name = "Profession")]
        public int ProfessionId { get; set; }
        public Profession? Profession { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        [Required]
        [ForeignKey("CourseLevel")]
        [Display(Name = "Course Level")]
        public int CourseLevelId { get; set; }
        public CourseLevel? CourseLevel { get; set; }

        [Required]
        [ForeignKey("Session")]
        public int SessionId { get; set; }
        public Session? Session { get; set; }

        
       

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [MaxLength(250)]
        public string? Notes { get; set; }

        
       
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Paid fee value")]
        public decimal PaidFeeValue { get; set; }

        [Display(Name = "Accept the registration terms")]
        public bool RegistrationTermsAccepted { get; set; }
        public bool RegistrationValidated { get; set; }
    }
}