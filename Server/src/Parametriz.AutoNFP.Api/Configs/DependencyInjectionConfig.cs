
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Application.Voluntarios.Services;
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository;
using Parametriz.AutoNFP.Data.Uow;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection RegistrarServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddScoped<IEmailService, EmailService>();


            services.AddScoped<IInstituicaoRepository, InstituicaoRepository>();
            services.AddScoped<IInstituicaoService, InstituicaoService>();

            services.AddScoped<IVoluntarioRepository, VoluntarioRepository>();
            services.AddScoped<IVoluntarioService, VoluntarioService>();

            services.AddScoped<Notificador>();

            services.AddScoped<AutoNfpDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}
