using CalisthenicsSkillTracker.Data.Seeding.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CalisthenicsSkillTracker.Infrastructure.Extensions;

public static class WebApplicationExtensions
{
    public static IApplicationBuilder UseRolesSeeder(this IApplicationBuilder builder) 
    {
        using IServiceScope scope = builder
            .ApplicationServices
            .CreateScope();

        IIdentitySeeder seeder = scope
            .ServiceProvider
            .GetRequiredService<IIdentitySeeder>();

        seeder
            .SeedRolesAsync()
            .GetAwaiter()
            .GetResult();

        return builder;
    }

    public static IApplicationBuilder UseAdminUserSeeder(this IApplicationBuilder builder) 
    {
        using IServiceScope scope = builder
            .ApplicationServices
            .CreateScope();

        IIdentitySeeder seeder = scope
            .ServiceProvider
            .GetRequiredService<IIdentitySeeder>();

        seeder
            .SeedAdminUserAsync()
            .GetAwaiter()
            .GetResult();

        return builder;
    }
}
