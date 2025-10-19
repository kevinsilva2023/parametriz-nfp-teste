using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.Docker
{
    public interface IDockerService
    {
        bool ExecutarProcessoInicial(string diretorio, string imageName, string containerName, int port);
        void ExecutarProcessoFinal(string containerName);
    }
}
