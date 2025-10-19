using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Voluntarios.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/voluntarios")]
    public class VoluntariosController : MainController
    {
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly IVoluntarioService _voluntarioService;

        public VoluntariosController(Notificador notificador,
                                     IAspNetUser user,
                                     IVoluntarioRepository voluntarioRepository,
                                     IVoluntarioService voluntarioService)
            : base(notificador, user)
        {
            _voluntarioRepository = voluntarioRepository;
            _voluntarioService = voluntarioService;
        }

        [HttpGet]
        public async Task<VoluntarioViewModel> Get()
        {
            return (await _voluntarioRepository.ObterPorInstituicaoIdAsync(InstituicaoId)).ToViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CadastrarVoluntarioViewModel cadastrarVoluntarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(cadastrarVoluntarioViewModel);

            var limiteUpload = 30 * 1024; // 30Kb. (No domain o limite é 25Kb por que na string base64 o calculo é aproximado)
            var tamanhoAproximadoUpload = cadastrarVoluntarioViewModel.Upload.Length * 0.75; // Convertido possui aproximadamente 1.37% 4/3

            if (tamanhoAproximadoUpload > limiteUpload)
            {
                NotificarErro("Arquivo excede o limite do tamanho permitido.");
                return CustomResponse(cadastrarVoluntarioViewModel);
            }
                
            await _voluntarioService.Cadastrar(cadastrarVoluntarioViewModel);

            return CustomResponse(cadastrarVoluntarioViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _voluntarioService.Excluir(InstituicaoId);

            return CustomResponse();
        }
    }
}
