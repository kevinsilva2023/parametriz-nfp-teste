using Parametriz.AutoNFP.Api.ViewModels.Identidade;

namespace Parametriz.AutoNFP.Api.Application.Instituicoes.Services
{
    public interface IInstituicaoService
    {
        Task<bool> Cadastrar(CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel, Guid voluntarioId);
    }
}
