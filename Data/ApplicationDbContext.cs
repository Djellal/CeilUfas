using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ceilufas.Models;

namespace Ceilufas.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Session> Sessions { get; set; } = default!;
    public DbSet<AppSetting> AppSettings { get; set; }= default!;
    public DbSet<State> States { get; set; }= default!;
    public DbSet<Municipality> Municipalities { get; set; }= default!;
    public DbSet<Profession> Professions { get; set; }= default!;
    public DbSet<Course> Courses { get; set; }= default!;
    public DbSet<CourseLevel> CourseLevels { get; set; }= default!;
    public DbSet<CourseRegistration> CourseRegistrations { get; set; }= default!;

}
