using Parametriz.AutoNFP.Api.ViewModels.Certificados;

namespace Parametriz.AutoNFP.Api.Application.Certificados.Services
{
    public interface ICertificadoService
    {
        Task<bool> Cadastrar(CadastrarCertificadoViewModel cadastrarCertificadoViewModel);
        Task<bool> Excluir(Guid voluntarioId);
    }
}
