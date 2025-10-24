using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Certificados.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.Utils;
using Parametriz.AutoNFP.Api.ViewModels.Certificados;
using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Certificados;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/certificados")]
    public class CertificadosController : MainController
    {
        private readonly ICertificadoRepository _certificadoRepository;
        private readonly ICertificadoService _certificadosService;

        public CertificadosController(Notificador notificador,
                                     IAspNetUser user,
                                     ICertificadoRepository certificadoRepository,
                                     ICertificadoService certificadoService)
            : base(notificador, user)
        {
            _certificadoRepository = certificadoRepository;
            _certificadosService = certificadoService;
        }

        [HttpGet("status")]
        public async Task<IEnumerable<EnumViewModel>> ObterStatus()
        {
            return await Task.FromResult(EnumUtils.ToViewModel(typeof(CertificadoStatus)));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CadastrarCertificadoViewModel cadastrarCertificadoViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(cadastrarCertificadoViewModel);

            var limiteUpload = 30 * 1024; // 30Kb. (No domain o limite é 25Kb por que na string base64 o calculo é aproximado)
            var tamanhoAproximadoUpload = cadastrarCertificadoViewModel.Upload.Length * 0.75; // Convertido possui aproximadamente 1.37% 4/3

            if (tamanhoAproximadoUpload > limiteUpload)
            {
                NotificarErro("Arquivo excede o limite do tamanho permitido.");
                return CustomResponse(cadastrarCertificadoViewModel);
            }
                
            await _certificadosService.Cadastrar(cadastrarCertificadoViewModel);

            return CustomResponse(cadastrarCertificadoViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _certificadosService.Excluir(VoluntarioId);

            return CustomResponse();
        }
    }
}
