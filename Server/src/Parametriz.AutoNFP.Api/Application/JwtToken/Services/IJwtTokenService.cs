using Parametriz.AutoNFP.Api.Models;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;

namespace Parametriz.AutoNFP.Api.Application.JwtToken.Services
{
    public interface IJwtTokenService
    {
        Task<LoginResponseViewModel> ObterLoginResponse(Guid instituicaoId, string email, RefreshToken token = null);

        Task<RefreshToken> ObterRefreshToken(Guid refreshToken);
    }
}
