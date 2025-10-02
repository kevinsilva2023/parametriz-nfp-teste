using Parametriz.AutoNFP.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Instituicoes
{
    public interface IInstituicaoRepository : IRepository<Instituicao>
    {
        Task<Guid> ObterIdPorVoluntarioId(Guid voluntarioId);
    }
}
