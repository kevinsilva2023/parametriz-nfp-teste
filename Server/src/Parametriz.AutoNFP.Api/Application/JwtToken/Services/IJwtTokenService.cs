using Parametriz.AutoNFP.Api.Models;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.JwtToken.Services
{
    public interface IJwtTokenService
    {
        Task<LoginResponseViewModel> ObterLoginResponse(Instituicao instituicao, Usuario usuario, RefreshToken token = null);

        Task<RefreshToken> ObterRefreshToken(Guid refreshToken);
    }
}
