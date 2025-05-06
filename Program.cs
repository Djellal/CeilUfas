<<<<<<< HEAD
=======
using Ceilufas.Components;
using Ceilufas.Components.Account;
using Ceilufas.Data;
using Ceilufas; // Add this line to access Globals class
>>>>>>> 50cc058c8be941e3b6019cb8ca7a2cac380967d0
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Ceilufas.Components;
using Ceilufas.Components.Account;
using Ceilufas.Data;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
<<<<<<< HEAD
builder.Services.AddDbContext<ApplicationDbContext>(options =>
=======
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
>>>>>>> 50cc058c8be941e3b6019cb8ca7a2cac380967d0
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Add this line to register RoleManager
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

// Seed roles if they don't exist
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    foreach (var role in Globals.Roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed admin user if it doesn't exist
    var adminUser = await userManager.FindByEmailAsync("djellal@univ-setif.dz");
    if (adminUser == null)
    {
        var user = new ApplicationUser
        {
            UserName = "djellal@univ-setif.dz",
            Email = "djellal@univ-setif.dz",
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(user, "DhB@571982");
        if (result.Succeeded)
        {
            Console.WriteLine($"Admin user created successfully: {user.Email}");
            await userManager.AddToRoleAsync(user, Globals.ADMIN);
        }
        else
        {
            Console.WriteLine($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();
