using Parametriz.AutoNFP.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Voluntarios
{
    public interface IVoluntarioRepository : IInstituicaoEntityRepository<Voluntario>
    {
        Task<bool> ExisteNaInstituicao(Guid instituicaoId);
        Task<Voluntario> ObterPorInstituicaoId(Guid instituicaoId);

        Task Excluir(Guid instituicaoId);
    }
}
