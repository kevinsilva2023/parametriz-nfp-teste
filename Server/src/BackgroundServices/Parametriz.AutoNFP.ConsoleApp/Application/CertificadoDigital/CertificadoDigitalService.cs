using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.ConsoleApp.Application.FileSistem;
using Parametriz.AutoNFP.Core.Configs;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.CertificadoDigital
{
    public class CertificadoDigitalService : BaseService, ICertificadoDigitalService
    {
        public CertificadoDigitalService(IUnitOfWork uow, 
                                         Notificador notificador) 
            : base(uow, notificador)
        {

        }

        public bool EstaValido(Certificado certificado, string senha)
        {
            try
            {
                var certificadoDigital = new X509Certificate2(certificado.Upload, senha);

                return true;
            }
            catch(CryptographicException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // ToDo: O que fazer quando certificado não está valido?
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
