using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.Usuarios.Services
{
    public interface IUsuarioService
    {
        Task<bool> Cadastrar(Usuario usuario);
    }
}
