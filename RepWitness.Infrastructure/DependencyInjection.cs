using Microsoft.Extensions.DependencyInjection;
using RepWitness.Application.Common.Interfaces;
using RepWitness.Infrastructure.Security;

namespace RepWitness.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
