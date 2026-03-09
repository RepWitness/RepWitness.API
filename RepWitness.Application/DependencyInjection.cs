using Microsoft.Extensions.DependencyInjection;
using RepWitness.Application.Common.Mappings;

namespace RepWitness.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(UserProfile).Assembly);

        return services;
    }
}