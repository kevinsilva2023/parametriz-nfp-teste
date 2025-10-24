using Parametriz.AutoNFP.Api.ViewModels.ErrosTransmissaoLote;
using Parametriz.AutoNFP.Domain.ErrosTransmissaoLote;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class ErroTransmissaoLoteExtensions
    {
        public static ErroTransmissaoLoteViewModel ToViewModel(this ErroTransmissaoLote erroTransmissaoLote)
        {
            return new ErroTransmissaoLoteViewModel
            {
                Id = erroTransmissaoLote.Id,
                InstituicaoId = erroTransmissaoLote.InstituicaoId,
                VoluntarioId = erroTransmissaoLote.VoluntarioId,
                Data = erroTransmissaoLote.Data,
                Mensagem = erroTransmissaoLote.Mensagem,
                Voluntario = erroTransmissaoLote.Voluntario.ToViewModel()
            };
        }

        public static IEnumerable<ErroTransmissaoLoteViewModel> ToViewModel(this IEnumerable<ErroTransmissaoLote> errosTransmissaoLote)
        {
            return errosTransmissaoLote.Select(e => e.ToViewModel());
        }
    }
}
