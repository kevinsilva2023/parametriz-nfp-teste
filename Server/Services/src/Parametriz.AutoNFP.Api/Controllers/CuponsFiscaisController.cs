using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.CuponsFiscais.Services;
using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.CuponsFiscais;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.CuponsFiscais;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/cupons-fiscais")]
    public class CuponsFiscaisController : MainController
    {
        private readonly ICupomFiscalRepository _cupomFiscalRepository;
        private readonly ICupomFiscalService _cupomFiscalService;

        public CuponsFiscaisController(Notificador notificador,
                                       IAspNetUser user,
                                       ICupomFiscalRepository cupomFiscalRepository,
                                       ICupomFiscalService cupomFiscalService)
            : base(notificador, user)
        {
            _cupomFiscalRepository = cupomFiscalRepository;
            _cupomFiscalService = cupomFiscalService;
        }

        [HttpGet("obter-por-usuario")]
        public async Task<IEnumerable<CupomFiscalViewModel>> ObterPorUsuario()
        {
            return (await _cupomFiscalRepository.ObterPorUsuarioId(UsuarioId, InstituicaoId))
                .ToViewModel();
        }

        [HttpGet]
        public async Task<CupomFiscalPaginacaoViewModel> ObterPorFiltrosPaginado(DateTime competencia, Guid? cadastradoPorId = null,
            CupomFiscalStatus? status = null, int pagina = 1, int registrosPorPagina = 50)
        {
            return (await _cupomFiscalRepository
                .ObterPorFiltrosPaginado(InstituicaoId, competencia, cadastradoPorId, status, pagina, registrosPorPagina))
                .ToViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CadastrarCupomFiscalViewModel cadastrarCupomFiscalViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse();

            await _cupomFiscalService.Cadastrar(cadastrarCupomFiscalViewModel);

            return CustomResponse();
        }
    }
}
