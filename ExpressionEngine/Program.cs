using ExpressionEngine.Api.Endpoints;
using ExpressionEngine.Api.Extensions;
using ExpressionEngine.Api.Interfaces;
using ExpressionEngine.Core.Interfaces;
using ExpressionEngine.Infrastructure.Data;
using ExpressionEngine.Infrastructure.Repositores;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

var MyAllowOrigins = "_myAllowOrigins";
builder.AddCors(MyAllowOrigins);

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
