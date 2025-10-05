using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Identity.Jwt;
using NetDevPack.Security.Jwt.Core;
using NetDevPack.Security.PasswordHasher.Core;
using Parametriz.AutoNFP.Api.Data.Context;
using Parametriz.AutoNFP.Api.Extensions.Identity;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddMemoryCache()
                .AddDataProtection();

            services.AddJwtConfiguration(configuration, "AppSettings")
                .AddNetDevPackIdentity<IdentityUser>()                
                .PersistKeysToDatabaseStore<ApplicationDbContext>();

            services.AddIdentity<IdentityUser, IdentityRole>(o =>
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
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.UpgradePasswordSecurity()
                .WithStrengthen(PasswordHasherStrength.Moderate)
                .UseArgon2<IdentityUser>();

            return services;
        }
    }
}
