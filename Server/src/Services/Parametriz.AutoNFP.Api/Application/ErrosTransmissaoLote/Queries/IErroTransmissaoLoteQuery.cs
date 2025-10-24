using Parametriz.AutoNFP.Api.ViewModels.ErrosTransmissaoLote;

namespace Parametriz.AutoNFP.Api.Application.ErrosTransmissaoLote.Queries
{
    public interface IErroTransmissaoLoteQuery
    {
        Task<IEnumerable<ErroTransmissaoLoteViewModel>> ObterPorVoluntarioId(Guid voluntarioId, Guid instituicaoId);
    }
}
