using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;

        public VoluntarioQuery(IVoluntarioRepository voluntarioRepository, 
                               UserManager<IdentityUser> userManager)
        {
            _voluntarioRepository = voluntarioRepository;
            _userManager = userManager;
        }

        public async Task<IEnumerable<VoluntarioViewModel>> ObterPorFiltros(Guid instituicaoId, string nome = "", string email = "", 
            CertificadoStatus? certificadoStatus = null, BoolTresEstados administrador = BoolTresEstados.Ambos, 
            BoolTresEstados desativado = BoolTresEstados.Falso)
        {
            var voluntarios = await _voluntarioRepository.ObterPorFiltros(instituicaoId, nome, email, administrador, desativado);

            var voluntariosViewModel = voluntarios
                .Where(v => certificadoStatus == null || v.Certificado?.Status == certificadoStatus)
                .ToList()
                .ToViewModel();

            foreach (var voluntarioViewModel in voluntariosViewModel)
            {
                var user = await _userManager.FindByIdAsync(voluntarioViewModel.Id.ToString());
                voluntarioViewModel.EmailConfirmado = user.EmailConfirmed;
            }

            return voluntariosViewModel;
        }
    }
}
