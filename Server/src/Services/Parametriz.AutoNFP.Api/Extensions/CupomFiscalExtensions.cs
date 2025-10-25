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
                Competencia = cupomFiscal.ChaveDeAcesso.EmitidoEm.Value,
                Numero = cupomFiscal.ChaveDeAcesso.Numero,
                Cnpj = cupomFiscal.ChaveDeAcesso.Cnpj.NumeroInscricao,
                CadastradoPorId = cupomFiscal.CadastradoPorId,
                CadastradoEm = cupomFiscal.CadastradoEm,
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

        public static CupomFiscalPaginacaoViewModel ToViewModel(this CupomFiscalPaginacao cupomFiscalPaginacao)
        {
            return new CupomFiscalPaginacaoViewModel
            {
                CuponsFiscais = cupomFiscalPaginacao.CuponsFiscais.ToViewModel(),
                Pagina = cupomFiscalPaginacao.Pagina,
                RegistrosPorPagina = cupomFiscalPaginacao.RegistrosPorPagina,
                Processando = cupomFiscalPaginacao.Processando,
                Sucesso = cupomFiscalPaginacao.Sucesso,
                Erro = cupomFiscalPaginacao.Erro,
                Total = cupomFiscalPaginacao.Total
            };
        }
    }
}
