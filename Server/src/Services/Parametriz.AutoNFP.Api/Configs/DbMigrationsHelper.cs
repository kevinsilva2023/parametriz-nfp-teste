using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parametriz.AutoNFP.Api.Data;
using Parametriz.AutoNFP.Data.Context;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class DbMigrationsHelper
    {
        public static async Task EnsureSeedData(WebApplication app)
        {
            var serviceProvider = app.Services.CreateScope().ServiceProvider;
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
              
            await EnsureSeedDataAutoNfpIdentityDbContext(scope, env);
            await EnsureSeedDataAutoNfpDbContext(scope, env);
        }

        public static async Task EnsureSeedDataAutoNfpIdentityDbContext(IServiceScope scope, IWebHostEnvironment env)
        {
            var autoNfpIdentityDbContext = scope.ServiceProvider.GetRequiredService<AutoNfpIdentityDbContext>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await DbHealthChecker.TestConnection(autoNfpIdentityDbContext);

            if (env.IsDevelopment())
                await autoNfpIdentityDbContext.Database.MigrateAsync();

            await roleManager.CreateAsync(new IdentityRole("Administrador"));
            await roleManager.CreateAsync(new IdentityRole("Parametriz"));

            var parametrizUser = new IdentityUser()
            {
                UserName = "suporte@parametriz.com.br",
                Email = "suporte@parametriz.com.br",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(parametrizUser, "Parametriz@1314");
            await userManager.AddToRoleAsync(parametrizUser, "Parametriz");
        }

        private static async Task EnsureSeedDataAutoNfpDbContext(IServiceScope scope, IWebHostEnvironment env)
        {
            var autoNfpDbContext = scope.ServiceProvider.GetRequiredService<AutoNfpDbContext>();

            await DbHealthChecker.TestConnection(autoNfpDbContext);

            if (env.IsDevelopment())
                await autoNfpDbContext.Database.MigrateAsync();
        }
    }
}
