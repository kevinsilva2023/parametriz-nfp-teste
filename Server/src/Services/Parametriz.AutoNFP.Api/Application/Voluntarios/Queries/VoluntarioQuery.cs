using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Queries
{
    public class VoluntarioQuery : IVoluntarioQuery
    {
        private readonly IVoluntarioRepository _voluntarioRepository;

        public VoluntarioQuery(IVoluntarioRepository voluntarioRepository)
        {
            _voluntarioRepository = voluntarioRepository;
        }

        public async Task<IEnumerable<VoluntarioViewModel>> ObterPorFiltros(Guid instituicaoId, string nome = "", string email = "", 
            CertificadoStatus? certificadoStatus = null, BoolTresEstados administrador = BoolTresEstados.Ambos, 
            BoolTresEstados desativado = BoolTresEstados.Falso)
        {
            var voluntarios = await _voluntarioRepository.ObterPorFiltros(instituicaoId, nome, email, administrador, desativado);

            return voluntarios
                .Where(v => certificadoStatus == null || v.Certificado?.Status == certificadoStatus)
                .ToList()
                .ToViewModel();
        }
    }
}
