using CalisthenicsSkillTracker.Data.Models;
using CalisthenicsSkillTracker.Data.Seeding.Contracts;
using static CalisthenicsSkillTracker.GCommon.OutputMessages.IdentityMessages;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace CalisthenicsSkillTracker.Data.Seeding;

public class IdentitySeeder : IIdentitySeeder
{
    public static string[] _roles = { "Admin", "Moderator", "User"};

    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public IdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager,      
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration)
    {
        this._roleManager = roleManager;
        this._userManager = userManager;
        this._configuration = configuration;
    }


    public async Task SeedRolesAsync()
    {
        foreach (string role in _roles) 
        {
            if (!await this._roleManager.RoleExistsAsync(role)) 
            {
                IdentityResult result = await this._roleManager
                    .CreateAsync(new IdentityRole<Guid>(role));

                if (!result.Succeeded)
                    throw new InvalidOperationException(string
                        .Format(RoleSeedingExceptionMessage, role));
            }
        }
    }
    public async Task SeedAdminUserAsync()
    {
        string email = this._configuration["UserSeed:AdminUser:Email"]
            ?? throw new InvalidOperationException(AdminEmailSeedingExceptionMessage);

        string password = this._configuration["UserSeed:AdminUser:Password"]
            ?? throw new InvalidOperationException(AdminPasswordSeedingExceptionMessage);

        ApplicationUser? adminUser = await this._userManager.FindByEmailAsync(email);
        if (adminUser is null) 
        {
            adminUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FullName = "Admin User",
                DateOfBirth = DateTime.Now.AddYears(-18)
            };

            IdentityResult result = await this._userManager
                .CreateAsync(adminUser, password);
            if (!result.Succeeded)
                throw new InvalidOperationException(AdminUserSeedingExceptionMessage);
        }

        bool isInRole = await this._userManager.IsInRoleAsync(adminUser, _roles[0]);
        if (!isInRole) 
        {
            IdentityResult result = await this._userManager
                .AddToRoleAsync(adminUser, _roles[0]);

            if (!result.Succeeded)
                throw new InvalidOperationException(string
                    .Format(AdminUserRoleSeedingExceptionMessage, _roles[0]));
        }
    }
}
