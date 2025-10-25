using Parametriz.AutoNFP.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.ErrosTransmissaoLote
{
    public interface IErroTransmissaoLoteRepository : IInstituicaoEntityRepository<ErroTransmissaoLote>
    {
        bool Existe(Guid instituicaoId, Guid? voluntarioId, string mensagem);

        IEnumerable<ErroTransmissaoLote> ObterPorInstituicaoId(Guid instituicaoId);
        Task<IEnumerable<ErroTransmissaoLote>> ObterPorInstituicaoIdAsync(Guid instituicaoId);

        Task<IEnumerable<ErroTransmissaoLote>> ObterPorVoluntarioId(Guid voluntarioId, Guid instituicaoId);
    }
}
