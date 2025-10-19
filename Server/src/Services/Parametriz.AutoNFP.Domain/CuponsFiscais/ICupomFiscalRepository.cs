using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.CuponsFiscais
{
    public interface ICupomFiscalRepository : IInstituicaoEntityRepository<CupomFiscal>
    {
        Task<CupomFiscalPaginacao> ObterPorFiltrosPaginado(Guid instituicaoId, DateTime competencia, 
            Guid? cadastradoPorId = null, CupomFiscalStatus? status = null, int pagina = 1, int registrosPorPagina = 50);

        IEnumerable<Guid> ObterInstituicoesIdComCuponsFiscaisProcessando();
        IEnumerable<CupomFiscal> ObterPorStatusProcessando(Guid instituicaoId);
    }
}
