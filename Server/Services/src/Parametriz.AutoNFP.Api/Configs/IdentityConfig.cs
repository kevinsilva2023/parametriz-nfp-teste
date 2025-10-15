using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NetDevPack.Security.Jwt.Core.Jwa;
using NetDevPack.Security.JwtExtensions;
using NetDevPack.Security.PasswordHasher.Core;
using Parametriz.AutoNFP.Api.Application.JwtToken.Services;
using Parametriz.AutoNFP.Api.Data;
using Parametriz.AutoNFP.Api.Extensions.Identity;
using Parametriz.AutoNFP.Api.Models.User;
using System.Text;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class IdentityConfig
    {
        public static WebApplicationBuilder AddIdentityConfiguration(this WebApplicationBuilder builder)
        {
            builder
                .AddIdentityDbContext()
                .AddIdentityApiSupport()
                .AddJwtSupport();

            return builder;
        }

        private static WebApplicationBuilder AddIdentityDbContext(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AutoNfpIdentityDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            return builder;
        }

        private static WebApplicationBuilder AddIdentityApiSupport(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddMemoryCache()
                .AddDataProtection();

            builder.Services
                .AddJwksManager()
                .UseJwtValidation()
                .PersistKeysToDatabaseStore<AutoNfpIdentityDbContext>();

            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>(o =>
                {
                    o.SignIn.RequireConfirmedEmail = true;

                    o.Password.RequireDigit = true;
                    o.Password.RequireLowercase = true;
                    o.Password.RequireNonAlphanumeric = true;
                    o.Password.RequireUppercase = true;
                    o.Password.RequiredUniqueChars = 0;
                    o.Password.RequiredLength = 6;
                })
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddEntityFrameworkStores<AutoNfpIdentityDbContext>()
                .AddDefaultTokenProviders();

            builder.Services
                .UpgradePasswordSecurity()
                .WithStrengthen(PasswordHasherStrength.Moderate)
                .UseArgon2<IdentityUser>();

            return builder;
        }

        private static WebApplicationBuilder AddJwtSupport(this WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection("AppJwtConfig");
            builder.Services.Configure<AppJwtConfig>(appSettingsSection);

            var appJwtConfig = appSettingsSection.Get<AppJwtConfig>();

            // Caso seja necessário implementar JWKS
            //var appConfig = builder.Configuration.GetSection("AppConfig").Get<AppConfig>();

            builder.Services.AddAuthentication(
                p =>
                {
                    p.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    p.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = appJwtConfig.Audience,
                        ValidIssuer = appJwtConfig.Issuer
                    };
                    // Caso seja necessário implementar JWKS
                    //options.SetJwksOptions(new JwkOptions(appConfig.AutenticacaoJwksUrl));
                });

            return builder;
        }

        // ToDo: Autnticação Social: Verificar se vamos implementar
        public static WebApplicationBuilder AddSocialAuthenticationSupport(this WebApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddAuthentication()
                .AddFacebook(o =>
                {
                    o.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                    o.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                });

            return builder;
        }
    }
}
