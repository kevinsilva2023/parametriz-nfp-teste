using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.ErrosTransmissaoLote.Queries;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.ErrosTransmissaoLote;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.ErrosTransmissaoLote;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/erros-transmissao-lote")]
    public class ErrosTransmissaoLoteController : MainController
    {
        private readonly IErroTransmissaoLoteQuery _erroTransmissaoLoteQuery;

        public ErrosTransmissaoLoteController(Notificador notificador,
                                              IAspNetUser user,
                                              IErroTransmissaoLoteQuery erroTransmissaoLoteQuery)
            : base(notificador, user)
        {
            _erroTransmissaoLoteQuery = erroTransmissaoLoteQuery;
        }

        [HttpGet]
        public async Task<IEnumerable<ErroTransmissaoLoteViewModel>> Get()
        {
            return await _erroTransmissaoLoteQuery.ObterPorVoluntarioId(VoluntarioId, InstituicaoId);
        }
    }
}
