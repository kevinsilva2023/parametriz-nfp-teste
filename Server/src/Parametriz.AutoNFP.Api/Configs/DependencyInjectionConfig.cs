
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Uow;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegistrarServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAspNetUser, AspNetUser>();
            

            services.AddScoped<Notificador>();


            services.AddScoped<AutoNfpDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}
