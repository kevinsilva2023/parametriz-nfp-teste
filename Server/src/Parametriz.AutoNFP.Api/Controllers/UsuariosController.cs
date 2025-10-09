using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/usuarios")]
    public class UsuariosController : MainController
    {
        public UsuariosController(Notificador notificador, IAspNetUser user) : base(notificador, user)
        {
        }

        
    }
}
