using Microsoft.AspNetCore.Identity;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.Identidade.Services
{
    public interface IIdentidadeService
    {
        Task<bool> CadastrarInstituicao(CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel);
        Task EnviarConfirmarEmail(EnviarConfirmarEmailViewModel enviarConfirmarEmailViewModel);
        Task ConfirmarEmail(ConfirmarEmailViewModel confirmarEmailViewModel);
        Task EnviarDefinirSenha(EnviarDefinirSenhaViewModel enviarDefinirSenhaViewModel);
        Task<LoginResponseViewModel> DefinirSenha(DefinirSenhaViewModel definirSenhaViewModel);
        Task<LoginResponseViewModel> Login(LoginViewModel loginViewModel);
        Task<LoginResponseViewModel> UtilizarRefreshToken(Guid refreshToken);
        
        Task<bool> CadastrarVoluntario(VoluntarioViewModel voluntarioViewModel);
    }
}
