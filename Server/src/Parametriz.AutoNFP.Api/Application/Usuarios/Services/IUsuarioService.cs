using Parametriz.AutoNFP.Api.ViewModels.Identidade;

namespace Parametriz.AutoNFP.Api.Application.Usuarios.Services
{
    public interface IUsuarioService
    {
        Task<bool> Cadastrar(CadastrarUsuarioViewModel cadastrarVoluntarioViewModel);
    }
}
