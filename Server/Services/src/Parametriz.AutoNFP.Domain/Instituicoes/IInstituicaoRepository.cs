using Parametriz.AutoNFP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Instituicoes
{
    public interface IInstituicaoRepository : IRepository<Instituicao>
    {
        Task<Guid> ObterIdPorUsuarioId(Guid usuarioId);
        Task<Instituicao> ObterPorId(Guid id);
        Task<Instituicao> ObterPorUsuarioId(Guid usuarioId);
    }
}
