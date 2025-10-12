using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Voluntarios.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
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
            return (await _voluntarioRepository.ObterPorInstituicaoId(InstituicaoId)).ToViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CadastrarVoluntarioViewModel cadastrarVoluntarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(cadastrarVoluntarioViewModel);

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
