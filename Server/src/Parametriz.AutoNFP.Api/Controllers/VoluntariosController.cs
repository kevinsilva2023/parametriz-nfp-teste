using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/voluntarios")]
    public class VoluntariosController : MainController
    {
        private readonly IVoluntarioRepository _voluntarioRepository;

        public VoluntariosController(Notificador notificador, 
                                     IAspNetUser user) 
            : base(notificador, user)
        {
        }
    }
}
