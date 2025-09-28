
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegistrarServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddScoped<Notificador>();


            return services;
        }
    }
}
