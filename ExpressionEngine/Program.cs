using ExpressionEngine.Api.Endpoints;
using ExpressionEngine.Api.Extensions;
using ExpressionEngine.Api.Interfaces;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Infrastructure.Data;
using ExpressionEngine.Infrastructure.Repositores;
using ExpressionEngine.Infrastructure.Services;
using ExpressionEngine.Infrastructure.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

//TODO add test units

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var MyAllowOrigins = "_myAllowOrigins";
builder.AddCors(MyAllowOrigins);

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<IDateProvider, DateProvider>();
builder.Services.AddScoped<IExpressionValidator, ExpressionValidator>();
builder.Services.AddScoped<IExpressionTokensValidator, ExpressionTokensValidator>();
builder.Services.AddScoped<IExpressionFormatValidator, ExpressionFormatValidator>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IEndpointDefinition, OperationEndpoints>();
builder.Services.AddScoped<IOperationService, OperationService>();

var app = builder.Build();

app.UseCors(MyAllowOrigins);
using var scope = app.Services.CreateScope();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

var endpointDefinitions = scope.ServiceProvider.GetServices<IEndpointDefinition>();

foreach (var def in endpointDefinitions)
{
    def.MapEndpoints(app);
}

app.Run();
