using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AutoNfpDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.Configure<SmtpConfig>(configuration.GetSection("SmtpConfig"));
            services.Configure<UrlsConfig>(configuration.GetSection("UrlsConfig"));

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder =>
                        builder
                            .WithOrigins("http://localhost:4200")
                                .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader());


                options.AddPolicy("Production",
                    builder =>
                        builder
                            .WithOrigins("http://parametriz.gerencer.com.br")
                                .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("Development");

                // Por enquanto somente em development
                app.UseHttpsRedirection();
            }

            if (env.IsProduction())
                app.UseCors("Production");

            app.UseRouting();
            
            app.UseAuthConfiguration();

            app.UseJwksDiscovery();

            return app;
        }
    }
}
