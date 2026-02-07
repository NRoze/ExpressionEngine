using ExpressionEngine.Api.Endpoints;
using ExpressionEngine.Api.Interfaces;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Infrastructure;
using ExpressionEngine.Infrastructure.Repositores;
using ExpressionEngine.Infrastructure.Services;
using ExpressionEngine.Infrastructure.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExpressionEngine.Api.Extensions
{
    static public class AppBuilderExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public IServiceCollection AddCors(string policyName)
            {
                return builder.Services.AddCors(options =>
                {
                    var allowedOrigins = builder.Configuration
                        .GetSection("AllowedOrigins").Get<string[]>() ?? [];

                    options.AddPolicy(policyName, policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });
            }

            public IServiceCollection AddDbContext()
            {
                return builder.Services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            }

            public void AddValidators()
            {
                builder.Services.AddValidatorsFromAssemblyContaining<Program>();
                builder.Services.AddScoped<IExpressionValidator, ExpressionValidator>();
                builder.Services.AddScoped<IExpressionTokensValidator, ExpressionTokensValidator>();
                builder.Services.AddScoped<IExpressionFormatValidator, ExpressionFormatValidator>();
            }

            public void AddScopedDependencies()
            {
                builder.Services.AddScoped<IDateProvider, DateProvider>();
                builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
                builder.Services.AddScoped<IEndpointDefinition, OperationEndpoints>();
                builder.Services.AddScoped<IOperationService, OperationService>();
            }
        }

        extension(WebApplication app)
        {
            public void AddExceptionsFallback()
            {
                app.UseExceptionHandler(new ExceptionHandlerOptions
                {
                    ExceptionHandler = async context =>
                    {
                        var traceId = Activity.Current?.Id ?? context.TraceIdentifier;

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsJsonAsync(new ProblemDetails
                        {
                            Title = "Internal Server Error",
                            Status = 500,
                            Instance = traceId,
                        });
                    },
                    SuppressDiagnosticsCallback = _ => true
                }); 
            }

            public void MigrateDatabase()
            {
                using var scope = app.Services.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            public void MapEndpoints()
            {
                using var scope = app.Services.CreateScope();

                var endpointDefinitions = scope.ServiceProvider.GetServices<IEndpointDefinition>();
                foreach (var def in endpointDefinitions)
                {
                    def.MapEndpoints(app);
                }
                
                app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
            }
        }
    }
}
