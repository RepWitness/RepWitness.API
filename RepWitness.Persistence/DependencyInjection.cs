using Microsoft.Extensions.DependencyInjection;
using RepWitness.Domain.Interfaces;
using RepWitness.Persistence.Repositories;

namespace RepWitness.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        return services;
    }
}
