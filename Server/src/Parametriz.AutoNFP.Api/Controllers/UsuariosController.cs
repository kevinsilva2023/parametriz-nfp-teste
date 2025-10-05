using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/usuarios")]
    public class UsuariosController : MainController
    {
        public UsuariosController(Notificador notificador, IAspNetUser user) : base(notificador, user)
        {
        }

        [AllowAnonymous]
        [HttpGet("allow-anonymous")]
        public async Task<string> AllowAnonimous()
        {
            return await Task.FromResult("Teste allowanonimous");
        }

        [HttpGet("authorize")]
        public async Task<string> Authorize()
        {
            return await Task.FromResult("Teste authorize");
        }
    }
}
