using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Instituicoes;

namespace Parametriz.AutoNFP.Api.Application.Instituicoes.Services
{
    public interface IInstituicaoService
    {
        Task<bool> Cadastrar(CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel, Guid userId);
        Task<bool> Atualizar(InstituicaoViewModel instituicaoViewModel);
        Task<bool> Desativar(Guid id);
        Task<bool> Ativar(Guid id);
    }
}
