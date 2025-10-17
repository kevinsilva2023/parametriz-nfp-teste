using Parametriz.AutoNFP.ConsoleApp.Application.CertificadoDigital;
using Parametriz.AutoNFP.ConsoleApp.Application.CuponsFiscais;
using Parametriz.AutoNFP.ConsoleApp.Application.FileSistem;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.Application.EnviarCuponsFiscais
{
    public class EnviarCuponsFiscaisService : BaseService, IEnviarCuponsFiscaisService
    {
        private readonly ICupomFiscalRepository _cupomFiscalRepository;
        private readonly ICupomFiscalService _cupomFiscalService;
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly ICertificadoDigitalService _certificadoDigitalService;
        private readonly IFileSystemService _fileSystemService;

        public EnviarCuponsFiscaisService(IUnitOfWork uow, 
                                          Notificador notificador) 
            : base(uow, notificador)
        {
        }

        public void ExecutarProcesso()
        {
            var instituicoesId = _cupomFiscalRepository.ObterInstituicoesIdComCuponsFiscaisProcessando();

            foreach (var instituicaoId in instituicoesId)
            {
                var cuponsFiscais = _cupomFiscalRepository.ObterPorStatusProcessando(instituicaoId);

                if (cuponsFiscais.Count() <= 0)
                    continue;

                if (!_certificadoDigitalService.ExecutarProcesso(instituicaoId))
                    continue;
                
            }
        }
    }
}
