using Microsoft.Extensions.DependencyInjection;
using RepWitness.Application.Common.Interfaces;
using RepWitness.Infrastructure.Interfaces;
using RepWitness.Infrastructure.Security;
using RepWitness.Infrastructure.Services;

namespace RepWitness.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}
