using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Domain.Certificados;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Queries
{
    public interface IVoluntarioQuery
    {
        Task<IEnumerable<VoluntarioViewModel>> ObterPorFiltros(Guid instituicaoId, string nome = "", string email = "", 
            CertificadoStatus? certificadoStatus = null, BoolTresEstados administrador = BoolTresEstados.Ambos, 
            BoolTresEstados desativado = BoolTresEstados.Falso);
    }
}
