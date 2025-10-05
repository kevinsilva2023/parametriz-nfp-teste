using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class ApiConfig
    {
        public static WebApplicationBuilder AddApiConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddControllers();

            builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));
            builder.Services.Configure<UrlsConfig>(builder.Configuration.GetSection("UrlsConfig"));

            builder.Services.AddCors(options =>
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

                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return builder;
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.UseJwksDiscovery();

            return app;
        }
    }
}
