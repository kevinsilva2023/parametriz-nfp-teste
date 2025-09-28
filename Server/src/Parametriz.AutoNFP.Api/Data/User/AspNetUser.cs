using System.Data;
using System.Security.Claims;

namespace Parametriz.AutoNFP.Api.Data.User
{
    public class AspNetUser : IAspNetUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public Guid ObterId()
        {
            return EstaAutenticado() ? Guid.Parse(_accessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string ObterEmail()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserEmail() : string.Empty;
        }

        public string ObterToken()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserToken() : string.Empty;
        }

        public string ObterRefreshToken()
        {
            return EstaAutenticado() ? _accessor.HttpContext.User.GetUserRefreshToken() : string.Empty;
        }

        public bool EstaAutenticado()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool PossuiRole(string role)
        {
            return _accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> ObterClaims()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public HttpContext ObterHttpContext()
        {
            return _accessor.HttpContext;
        }
    }
}
