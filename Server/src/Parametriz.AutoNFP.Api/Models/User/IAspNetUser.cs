using System.Security.Claims;

namespace Parametriz.AutoNFP.Api.Models.User
{
    public interface IAspNetUser
    {
        string Name { get; }
        Guid ObterId();
        string ObterEmail();
        string ObterToken();
        Guid ObterInstituicaoId();
        bool EstaAutenticado();
        bool PossuiRole(string role);
        IEnumerable<Claim> ObterClaims();

        HttpContext ObterHttpContext();
    }
}
