using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CalisthenicsSkillTracker.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection collection,
         Type repositoryType) 
    {
        Assembly repositoriesAssembly = repositoryType.Assembly;

        IEnumerable<Type> interfaces = repositoriesAssembly
            .GetTypes()
            .Where(t => t.IsInterface &&
                t.Name.StartsWith("I") && t.Name.EndsWith("Repository"))
            .ToArray();

        foreach (Type serviceType in interfaces) 
        {
            Type implementationType = repositoriesAssembly
                .GetTypes()
                .Single(t => t is { IsClass: true, IsAbstract: false } &&
                    serviceType.IsAssignableFrom(t));

            collection.AddScoped(serviceType, implementationType);
        }

        return collection;
    }

    public static IServiceCollection RegisterServices(this IServiceCollection collection,
         Type repositoryType)
    {
        Assembly repositoriesAssembly = repositoryType.Assembly;

        IEnumerable<Type> interfaces = repositoriesAssembly
            .GetTypes()
            .Where(t => t.IsInterface &&
                t.Name.StartsWith("I") && t.Name.EndsWith("Service"))
            .ToArray();

        foreach (Type serviceType in interfaces)
        {
            Type implementationType = repositoriesAssembly
                .GetTypes()
                .Single(t => t is { IsClass: true, IsAbstract: false } &&
                    serviceType.IsAssignableFrom(t));

            collection.AddScoped(serviceType, implementationType);
        }

        return collection;
    }
}
