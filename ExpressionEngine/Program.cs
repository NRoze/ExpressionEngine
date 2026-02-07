using ExpressionEngine.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var MyAllowOrigins = "AllowOriginsExpressionEngine";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.AddCors(MyAllowOrigins);
builder.AddDbContext();
builder.AddValidators();
builder.AddScopedDependencies();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.AddExceptionsFallback();
app.MapScalarApiReference();
app.UseCors(MyAllowOrigins);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MigrateDatabase();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapEndpoints(); 
app.Run();
