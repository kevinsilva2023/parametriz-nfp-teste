using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.CuponsFiscais
{
    public interface ICupomFiscalRepository : IInstituicaoEntityRepository<CupomFiscal>
    {
        Task<CupomFiscalPaginacao> ObterPorFiltrosPaginado(Guid instituicaoId, DateTime cadastradoEm, DateTime? emitidoEm = null, 
            Guid? cadastradoPorId = null, CupomFiscalStatus? status = null, int pagina = 1, int registrosPorPagina = 50);

        IEnumerable<Instituicao> ObterInstituicoesComCuponsFiscaisProcessando();
        IEnumerable<Voluntario> ObterVoluntariosComCuponsFiscaisProcessando(Guid instituicaoId);
        IEnumerable<CupomFiscal> ObterPorStatusProcessando(Guid voluntarioId, Guid instituicaoId);
    }
}
