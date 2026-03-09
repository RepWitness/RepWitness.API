using Microsoft.EntityFrameworkCore;
using RepWitness.Application;
using RepWitness.Infrastructure;
using RepWitness.Persistence;
using RepWitness.Persistence.Context;
using System.Reflection;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Assembly[] allCoreProjectsAssembly =
       [
           typeof(RepWitness.Application.DependencyInjection).Assembly
       ];

        builder.Services.AddControllers();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(allCoreProjectsAssembly));
        builder.Services.AddInfrastructureServices();
        builder.Services.AddOpenApi();
        builder.Services.AddApplicationConfiguration();
        builder.Services.AddPersistenceServices();
        builder.Services.AddDbContext<RepWitnessContext>(options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("RepWitness"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}