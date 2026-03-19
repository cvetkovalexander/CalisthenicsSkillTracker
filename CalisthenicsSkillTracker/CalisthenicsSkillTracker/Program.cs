using CalisthenicsSkillTracker.Data;
using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Repositories;
using CalisthenicsSkillTracker.Data.Repositories.Contracts;
using CalisthenicsSkillTracker.Infrastructure.Extensions;
using CalisthenicsSkillTracker.Services.Core.Interfaces;
using CalisthenicsSkillTracker.Services.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CalisthenicsSkillTracker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.RegisterRepositories(typeof(WorkoutRepository));
        builder.Services.RegisterServices(typeof(WorkoutService));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            ConfigureIdentity(options, builder.Configuration);
        })
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }

    private static void ConfigureIdentity(IdentityOptions options, ConfigurationManager configuration) 
    {
        /* Development settings */

        // Sign in settings
        options.SignIn.RequireConfirmedAccount = configuration
            .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedAccount");
        options.SignIn.RequireConfirmedEmail = configuration
            .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedEmail"); ;
        options.SignIn.RequireConfirmedPhoneNumber = configuration
            .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedPhoneNumber");

        //User settings
        options.User.RequireUniqueEmail = configuration
            .GetValue<bool>("IdentityOptions:User:RequireUniqueEmail");

        // Locout settings
        options.Lockout.MaxFailedAccessAttempts = configuration
            .GetValue<int>("IdentityOptions:Lockout:MaxFailedAttempts");
        options.Lockout.DefaultLockoutTimeSpan = configuration
            .GetValue<TimeSpan>("IdentityOptions:Lockout:DefaultLockoutTimeSpan");

        // Password settings - security is not requred for development
        options.Password.RequireDigit = configuration
            .GetValue<bool>("IdentityOptions:Password:RequireDigit"); ;
        options.Password.RequireLowercase = configuration
            .GetValue<bool>("IdentityOptions:Password:RequireLowercase");
        options.Password.RequireUppercase = configuration
            .GetValue<bool>("IdentityOptions:Password:RequireConfirmedAccount");
        options.Password.RequireNonAlphanumeric = configuration
            .GetValue<bool>("IdentityOptions:Password:RequireNonAlphanumeric");
        options.Password.RequiredLength = configuration
            .GetValue<int>("IdentityOptions:Password:RequiredLength");
        options.Password.RequiredUniqueChars = configuration
            .GetValue<int>("IdentityOptions:Password:RequiredUniqueChars");
    }
}
