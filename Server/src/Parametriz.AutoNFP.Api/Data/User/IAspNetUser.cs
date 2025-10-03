using System.Security.Claims;

namespace Parametriz.AutoNFP.Api.Data.User
{
    public interface IAspNetUser
    {
        string Name { get; }
        Guid ObterId();
        string ObterEmail();
        string ObterToken();
        string ObterRefreshToken();
        Guid ObterInstituicaoId();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ObterClaims();

        HttpContext ObterHttpContext();
    }
}
