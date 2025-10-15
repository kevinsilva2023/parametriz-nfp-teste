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
        Task<IEnumerable<CupomFiscal>> ObterPorUsuarioId(Guid usuarioId, Guid instituicaoId);
    }
}
