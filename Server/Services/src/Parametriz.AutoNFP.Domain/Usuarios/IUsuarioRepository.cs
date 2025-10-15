using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Usuarios
{
    public interface IUsuarioRepository : IInstituicaoEntityRepository<Usuario>
    {
        
        Task<bool> ExistemOutrosUsuariosNaInstituicao(Guid id, Guid instituicaoId);
        Task<bool> ExistemOutrosAdministradoresNaInstituicao(Guid id, Guid instituicaoId);
        Task<IEnumerable<Usuario>> ObterPorFiltros(Guid instituicaoId, string nome = "", string email = "", 
            BoolTresEstados administrador = BoolTresEstados.Ambos, BoolTresEstados desativado = BoolTresEstados.Falso);
    }
}
