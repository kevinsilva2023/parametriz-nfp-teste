using Parametriz.AutoNFP.Api.ViewModels.CuponsFiscais;
using Parametriz.AutoNFP.Domain.CuponsFiscais;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class CupomFiscalExtensions
    {
        public static CupomFiscalViewModel ToViewModel(this CupomFiscal cupomFiscal)
        {
            return new CupomFiscalViewModel 
            {
                Id = cupomFiscal.Id,
                InstituicaoId = cupomFiscal.InstituicaoId,
                Chave = cupomFiscal.ChaveDeAcesso.Chave,
                MesEmissao = cupomFiscal.ChaveDeAcesso.MesEmissao.Value,
                Cnpj = cupomFiscal.ChaveDeAcesso.Cnpj.NumeroInscricao,
                CadastradoPorId = cupomFiscal.CadastradoPorId,
                Status = cupomFiscal.Status,
                EnviadoEm = cupomFiscal.EnviadoEm,
                MensagemErro = cupomFiscal.MensagemErro,

                CadastradoPor = cupomFiscal.CadastradoPor.ToViewModel(),
            };
        }

        public static IEnumerable<CupomFiscalViewModel> ToViewModel(this IEnumerable<CupomFiscal> cuponsFiscais)
        {
            return cuponsFiscais.Select(c => c.ToViewModel());
        }
    }
}
