using Parametriz.AutoNFP.Api.ViewModels.CuponsFiscais;

namespace Parametriz.AutoNFP.Api.Application.CuponsFiscais.Services
{
    public interface ICupomFiscalService
    {
        Task<bool> Cadastrar(CadastrarCupomFiscalViewModel cadastrarCupomFiscalViewModel);
    }
}
