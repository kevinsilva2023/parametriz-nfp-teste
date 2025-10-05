
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
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null) 
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IInstituicaoRepository, InstituicaoRepository>();
            builder.Services.AddScoped<IInstituicaoService, InstituicaoService>();

            builder.Services.AddScoped<IVoluntarioRepository, VoluntarioRepository>();
            builder.Services.AddScoped<IVoluntarioService, VoluntarioService>();

            builder.Services.AddScoped<Notificador>();

            builder.Services.AddScoped<AutoNfpDbContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return builder;
        }
    }
}
