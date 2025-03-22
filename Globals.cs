using CeilUfas.Data;
using CeilUfas.Models;
using CeilUfas.Models.ceilufas;
using Microsoft.AspNetCore.Identity;

namespace CeilUfas
{
    public class Globals
    {
        public const string Admin = "Admin";
        public const string Teacher = "Teacher";
        public const string Student = "Student";

        public static AppSetting appSettings { get; set; }

        public static async Task EnsureParamAsync(IServiceProvider serviceProvider)
        {


            string[] roleNames = { Admin, Teacher, Student };

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();


            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new ApplicationRole();
                    role.Name = roleName;
                    await roleManager.CreateAsync(role);
                }
            }

            // Optionally create an admin user if needed
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminEmail = "djellal@univ-setif.dz";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "DhB@571982");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            var dbcontext = serviceProvider.GetRequiredService<ceilufasContext>();

            appSettings = dbcontext.AppSettings.FirstOrDefault();
            if (appSettings == null)
            {
                appSettings = new AppSetting
                {
                    Id = 1,
                    OrganizationName = "CEIL de l’UFAS1",
                    Email = "ceil@univ-setif.dz",
                    Tel = "(+213) 036.62.09.96",
                    Logo = "",
                    Address = "Université Sétif -1-\r\nCampus El Bez, \r\nEx-Faculté de Droit (Actuellement Département d'Agronomie)",
                    WebSite = "https://ceil.univ-setif.dz",
                    Fb = "https://www.facebook.com/CEIL.SETIF1UNIVERSITY",
                    LinkredIn = "https://www.linkedin.com/school/universite-ferhat-abbas-setif",
                    IsRegistrationOpened = false
                };
                dbcontext.Add(appSettings);
                await dbcontext.SaveChangesAsync();
            }
        }
    }
}
