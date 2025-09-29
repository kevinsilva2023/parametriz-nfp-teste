using Parametriz.AutoNFP.Api.Configs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Api.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

app.MapControllers();

app.Run();
