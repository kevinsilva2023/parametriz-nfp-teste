using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Instituicoes;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/instituicoes")]
    public class InstituicoesController : MainController
    {
        private readonly IInstituicaoRepository _instituicaoRepository;
        private readonly IInstituicaoService _instituicaoService;

        public InstituicoesController(Notificador notificador,
                                      IAspNetUser user,
                                      IInstituicaoRepository instituicaoRepository,
                                      IInstituicaoService instituicaoService)
            : base(notificador, user)
        {
            _instituicaoRepository = instituicaoRepository;
            _instituicaoService = instituicaoService;
        }

        [HttpGet]
        public async Task<ActionResult<InstituicaoViewModel>> Get()
        {
            var instituicaoViewModel = (await _instituicaoRepository.ObterPorId(InstituicaoId)).ToViewModel();

            if (instituicaoViewModel == null)
                return NotFound();

            return instituicaoViewModel;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] InstituicaoViewModel instituicaoViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _instituicaoService.Atualizar(instituicaoViewModel);

            return CustomResponse(instituicaoViewModel);
        }

        [Authorize(Roles = "Parametriz")]
        [HttpPut("desativar/{id:guid}")]
        public async Task<IActionResult> Desativar([FromBody] Guid id)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _instituicaoService.Desativar(id);

            return CustomResponse(id);
        }

        [Authorize(Roles = "Parametriz")]
        [HttpPut("ativar/{id:guid}")]
        public async Task<IActionResult> Ativar([FromBody] Guid id)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _instituicaoService.Ativar(id);

            return CustomResponse(id);
        }

    }
}
