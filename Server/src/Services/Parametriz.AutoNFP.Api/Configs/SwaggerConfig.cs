using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class SwaggerConfig
    {
        public static WebApplicationBuilder AddSwaggerConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null) 
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Parametriz - AutoNFP API",
                    Description = "API AutoNFP da Parametriz.",
                    Contact = new OpenApiContact() { Name = "Mauricio Kamikoga", Email = "suporte@parametriz.com.br" }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Insira o token JWT desta maneira: Bearer {seu token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                // ToDo: Quando a parte da autenticação estiver tudo pronto, configurar abaixo
                // Excluding ASP.NET Identity endpoints
                //s.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    var relativePath = apiDesc.RelativePath;

                //    // List of avoid patches
                //    var identityEndpoints = new[]
                //    {
                //        "register",
                //        "manage",
                //        "refresh",
                //        "login",
                //        "confirmEmail",
                //        "resendConfirmationEmail",
                //        "forgotPassword",
                //        "resetPassword"
                //    };

                //    // Validating if the endpoint is avoided
                //    foreach (var endpoint in identityEndpoints)
                //    {
                //        if (relativePath.Contains(endpoint, StringComparison.OrdinalIgnoreCase))
                //        {
                //            return false;
                //        }
                //    }

                //    return true;
                //});

            });

            return builder;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            if (app == null) 
                throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            return app;
        }
    }
}
