using Microsoft.EntityFrameworkCore;
using RepWitness.Application;
using RepWitness.Infrastructure;
using RepWitness.Infrastructure.Models;
using RepWitness.Persistence;
using RepWitness.Persistence.Context;
using Scalar.AspNetCore;
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
        builder.Services.AddOpenApi();
        builder.Services.Configure<SmtpSettings>(
            builder.Configuration.GetSection("SmtpSettings"));
        builder.Services.AddApplicationConfiguration();
        builder.Services.AddInfrastructureServices();
        builder.Services.AddPersistenceServices();
        builder.Services.AddDbContext<RepWitnessContext>(options => {
            options.UseSqlServer(builder.Configuration.GetConnectionString("RepWitness"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "RepWitness API V1");
            });
        }

        app.MapScalarApiReference(options =>
        {
            options.Title = "My API Docs";
            options.Theme = ScalarTheme.Purple;
            options.DefaultHttpClient = new(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}