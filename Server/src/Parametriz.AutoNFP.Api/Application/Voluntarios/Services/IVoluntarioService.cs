using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Services
{
    public interface IVoluntarioService
    {
        Task<bool> Cadastrar(CadastrarVoluntarioViewModel cadastrarVoluntarioViewModel);
    }
}
