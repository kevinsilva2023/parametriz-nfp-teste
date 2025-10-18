using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.CertificadoDigital
{
    public interface ICertificadoDigitalService
    {
        bool EstaValido(Voluntario voluntario, string senha);
    }
}
