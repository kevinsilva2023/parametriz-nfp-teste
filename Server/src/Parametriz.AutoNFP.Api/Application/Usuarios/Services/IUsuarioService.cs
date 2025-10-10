using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.Usuarios.Services
{
    public interface IUsuarioService
    {
        Task<bool> Cadastrar(UsuarioViewModel usuarioViewModel, Guid usuarioId);
        Task<bool> Atualizar(UsuarioViewModel usuarioViewModel);
        Task<bool> Desativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
