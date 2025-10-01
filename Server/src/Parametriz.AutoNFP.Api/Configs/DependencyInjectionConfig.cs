
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Application.Usuarios.Services;
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository;
using Parametriz.AutoNFP.Data.Uow;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Usuarios;

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

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<Notificador>();

            services.AddScoped<AutoNfpDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}
