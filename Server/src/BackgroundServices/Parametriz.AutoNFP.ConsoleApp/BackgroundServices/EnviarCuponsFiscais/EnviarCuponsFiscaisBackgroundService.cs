using Microsoft.Extensions.DependencyInjection;
using Parametriz.AutoNFP.ConsoleApp.Application.CuponsFiscais;
using Parametriz.AutoNFP.ConsoleApp.Application.EnviarCuponsFiscais;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.BackgroundServices.EnviarCuponsFiscais
{
    public class EnviarCuponsFiscaisBackgroundService : BaseBackgroundService
    {
        public EnviarCuponsFiscaisBackgroundService(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        protected override void DoWork(object state)
        {
            Console.WriteLine($"Iniciando envio de cupons fiscais.");

            using var scope = _serviceProvider.CreateScope();

            var enviarCuponsFiscaisService = scope.ServiceProvider.GetRequiredService<IEnviarCuponsFiscaisService>();

            enviarCuponsFiscaisService.ExecutarProcesso();

            Console.WriteLine($"Fim do envio de cupons fiscais.");
        }
    }
}
