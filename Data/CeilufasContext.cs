using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CeilUfas.Models.ceilufas;

namespace CeilUfas.Data
{
    public partial class ceilufasContext : DbContext
    {
        public ceilufasContext()
        {
        }

        public ceilufasContext(DbContextOptions<ceilufasContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CeilUfas.Models.ceilufas.AppSetting>()
              .HasOne(i => i.Session)
              .WithMany(i => i.AppSettings)
              .HasForeignKey(i => i.CurrentSessionId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseLevel>()
              .HasOne(i => i.Course)
              .WithMany(i => i.CourseLevels)
              .HasForeignKey(i => i.CourseId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseLevel>()
              .HasOne(i => i.CourseLevel1)
              .WithMany(i => i.CourseLevels1)
              .HasForeignKey(i => i.PreviousCourseLevelId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseRegistration>()
              .HasOne(i => i.Municipality)
              .WithMany(i => i.CourseRegistrations)
              .HasForeignKey(i => i.BirthMunicipalityId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseRegistration>()
              .HasOne(i => i.State)
              .WithMany(i => i.CourseRegistrations)
              .HasForeignKey(i => i.BirthStateId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseRegistration>()
              .HasOne(i => i.Course)
              .WithMany(i => i.CourseRegistrations)
              .HasForeignKey(i => i.CourseId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseRegistration>()
              .HasOne(i => i.CourseLevel)
              .WithMany(i => i.CourseRegistrations)
              .HasForeignKey(i => i.CourseLevelId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseRegistration>()
              .HasOne(i => i.Profession)
              .WithMany(i => i.CourseRegistrations)
              .HasForeignKey(i => i.ProfessionId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.CourseRegistration>()
              .HasOne(i => i.Session)
              .WithMany(i => i.CourseRegistrations)
              .HasForeignKey(i => i.SessionId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.Course>()
              .HasOne(i => i.CourseType)
              .WithMany(i => i.Courses)
              .HasForeignKey(i => i.CourseTypeId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.Municipality>()
              .HasOne(i => i.State)
              .WithMany(i => i.Municipalities)
              .HasForeignKey(i => i.StateId)
              .HasPrincipalKey(i => i.Id);

            builder.Entity<CeilUfas.Models.ceilufas.Course>()
              .Property(p => p.Image)
              .HasDefaultValueSql(@"''::text");

            builder.Entity<CeilUfas.Models.ceilufas.CourseRegistration>()
              .Property(p => p.PaidFeeValue)
              .HasPrecision(18,2);

            builder.Entity<CeilUfas.Models.ceilufas.Profession>()
              .Property(p => p.FeeValue)
              .HasPrecision(18,2);
            this.OnModelBuilding(builder);
        }

        public DbSet<CeilUfas.Models.ceilufas.AppSetting> AppSettings { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.CourseLevel> CourseLevels { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.CourseRegistration> CourseRegistrations { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.Course> Courses { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.CourseType> CourseTypes { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.Municipality> Municipalities { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.Profession> Professions { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.Session> Sessions { get; set; }

        public DbSet<CeilUfas.Models.ceilufas.State> States { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}