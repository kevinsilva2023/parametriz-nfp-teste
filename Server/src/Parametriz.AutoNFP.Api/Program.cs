using Parametriz.AutoNFP.Api.Configs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddApiConfiguration(builder.Configuration);
builder.Services.RegistrarServices(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
DbMigrationsHelper.EnsureSeedData(app).Wait();
app.UseSwaggerConfiguration();
app.UseApiConfiguration(app.Environment);
app.MapControllers();

app.Run();
