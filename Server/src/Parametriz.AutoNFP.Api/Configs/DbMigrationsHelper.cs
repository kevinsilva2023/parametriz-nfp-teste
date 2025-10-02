using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Parametriz.AutoNFP.Api.Data.Context;
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
                        
            await EnsureSeedDataApplicationDbContext(scope, env);
            await EnsureSeedDataAutoNfpDbContext(scope, env);
        }

        public static async Task EnsureSeedDataApplicationDbContext(IServiceScope scope, IWebHostEnvironment env)
        {
            var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await DbHealthChecker.TestConnection(applicationDbContext);

            if (env.IsDevelopment())
                await applicationDbContext.Database.MigrateAsync();                
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
