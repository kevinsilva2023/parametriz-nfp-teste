using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Usuarios
{
    public interface IVoluntarioRepository : IInstituicaoEntityRepository<Voluntario>
    {
        Task<bool> ExistemOutrosVoluntariosNaInstituicao(Guid id, Guid instituicaoId);
        Task<bool> ExistemOutrosAdministradoresNaInstituicao(Guid id, Guid instituicaoId);
        Task<IEnumerable<Voluntario>> ObterPorFiltros(Guid instituicaoId, string nome = "", string email = "", 
            BoolTresEstados administrador = BoolTresEstados.Ambos, BoolTresEstados desativado = BoolTresEstados.Falso);
        Task<IEnumerable<Voluntario>> ObterAtivos(Guid instituicaoId);
    }
}
