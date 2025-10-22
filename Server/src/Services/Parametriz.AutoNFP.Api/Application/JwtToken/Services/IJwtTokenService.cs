using Microsoft.AspNetCore.Identity;
using Parametriz.AutoNFP.Api.Models;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.JwtToken.Services
{
    public interface IJwtTokenService
    {
        Task<LoginResponseViewModel> ObterLoginResponse(Instituicao instituicao, Voluntario usuario, RefreshToken token = null);
        Task<LoginResponseViewModel> ObterParametrizLoginReponse(IdentityUser user);
        Task<RefreshToken> ObterRefreshToken(Guid refreshToken);
    }
}
