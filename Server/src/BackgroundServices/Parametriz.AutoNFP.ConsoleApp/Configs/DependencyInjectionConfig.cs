using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parametriz.AutoNFP.ConsoleApp.Application.CertificadoDigital;
using Parametriz.AutoNFP.ConsoleApp.Application.CuponsFiscais;
using Parametriz.AutoNFP.ConsoleApp.Application.EnviarCuponsFiscais;
using Parametriz.AutoNFP.ConsoleApp.Application.FileSistem;
using Parametriz.AutoNFP.ConsoleApp.BackgroundServices.EnviarCuponsFiscais;
using Parametriz.AutoNFP.Core.Configs;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository;
using Parametriz.AutoNFP.Data.Uow;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IHostBuilder AddDependencyInjectionConfiguration(this IHostBuilder builder)
        {
            if (builder == null) 
                throw new ArgumentNullException(nameof(builder));

            builder.ConfigureServices(services =>
            {
                services.AddScoped<IVoluntarioRepository, VoluntarioRepository>();
                services.AddScoped<ICupomFiscalRepository, CupomFiscalRepository>();
                services.AddScoped<ICupomFiscalService, CupomFiscalService>();
                
                services.AddScoped<ICertificadoDigitalService, CertificadoDigitalService>();
                services.AddScoped<IFileSystemService, FileSystemService>();
                services.AddScoped<IEnviarCuponsFiscaisService, EnviarCuponsFiscaisService>();

                services.AddHostedService<EnviarCuponsFiscaisBackgroundService>();

                services.AddScoped<Notificador>();
                
                services.AddScoped<AutoNfpDbContext>();
                services.AddScoped<IUnitOfWork, UnitOfWork>();
            });

            return builder;
        }
    }
}
