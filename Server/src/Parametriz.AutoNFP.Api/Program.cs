using Microsoft.AspNetCore.Identity;
using Parametriz.AutoNFP.Api.Configs;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

// Add services to the container.
builder
    .AddDatabaseConfiguration()
    .AddIdentityConfiguration()
    .AddApiConfiguration()
    .AddDependencyInjectionConfiguration()
    .AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
DbMigrationsHelper.EnsureSeedData(app).Wait();

app.UseApiConfiguration(app.Environment);
app.UseSwaggerConfiguration();

app.Run();
