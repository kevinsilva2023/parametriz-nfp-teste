using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Services
{
    public interface IVoluntarioService
    {
        Task<bool> Cadastrar(CadastrarVoluntarioViewModel cadastrarVoluntarioViewModel);
        Task<bool> Excluir(Guid instituicaoId);
    }
}
