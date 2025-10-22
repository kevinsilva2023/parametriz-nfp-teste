using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Services
{
    public interface IVoluntarioService
    {
        Task<bool> Cadastrar(VoluntarioViewModel voluntarioViewModel, Guid id);
        Task<bool> AtualizarPerfil(VoluntarioViewModel voluntarioViewModel);
        Task<bool> Atualizar(VoluntarioViewModel voluntarioViewModel);
        Task<bool> Desativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
