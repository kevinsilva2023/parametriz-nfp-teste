
using Parametriz.AutoNFP.Api.Application.CuponsFiscais.Services;
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Application.JwtToken.Services;
using Parametriz.AutoNFP.Api.Application.Usuarios.Services;
using Parametriz.AutoNFP.Api.Application.Voluntarios.Services;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository;
using Parametriz.AutoNFP.Data.Uow;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Usuarios;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Configs
{
    public static class DependencyInjectionConfig
    {
        public static WebApplicationBuilder AddDependencyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            if (builder == null) 
                throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddScoped<IAspNetUser, AspNetUser>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            builder.Services.AddScoped<IIdentidadeService, IdentidadeService>();

            builder.Services.AddScoped<IEmailService, EmailService>();

            builder.Services.AddScoped<IInstituicaoRepository, InstituicaoRepository>();
            builder.Services.AddScoped<IInstituicaoService, InstituicaoService>();

            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            builder.Services.AddScoped<IVoluntarioRepository, VoluntarioRepository>();
            builder.Services.AddScoped<IVoluntarioService, VoluntarioService>();

            builder.Services.AddScoped<ICupomFiscalRepository, CupomFiscalRepository>();
            builder.Services.AddScoped<ICupomFiscalService, CupomFiscalService>();

            builder.Services.AddScoped<Notificador>();

            builder.Services.AddScoped<AutoNfpDbContext>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return builder;
        }
    }
}
