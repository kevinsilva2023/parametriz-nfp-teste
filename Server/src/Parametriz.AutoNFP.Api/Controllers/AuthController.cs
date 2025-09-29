using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NetDevPack.Identity.Interfaces;
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Usuarios.Services;
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Usuarios;
using System.Text;
using System.Text.Encodings.Web;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(Notificador notificador,
                              IAspNetUser user,
                              IJwtBuilder jwtBuilder,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IEmailService emailService)
            : base(notificador, user)
        {
            _jwtBuilder = jwtBuilder;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("novo-usuario")]
        public async Task<IActionResult> Post([FromBody] NovoUsuarioViewModel novoUsuario)
        {
            if (!ModelStateValida())
                return CustomResponse(novoUsuario);

            var user = new IdentityUser { UserName = novoUsuario.Email, Email = novoUsuario.Email };
            var result = await _userManager.CreateAsync(user, novoUsuario.Senha);

            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(user.Email);

                if (!await CadastrarUsuario(user))
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(novoUsuario);
                }

                await EnviarLinkConfirmarEmail(user);
            }

            AdicionarErrosIdentity(result);

            return CustomResponse(novoUsuario);
        }
        

        private async Task<bool> CadastrarUsuario(IdentityUser user)
        {
            var usuario = new Usuario(Guid.Parse(user.Id), null, new Email(user.Email));

            return await _usuarioService.Cadastrar(usuario);
        }

        private async Task EnviarLinkConfirmarEmail(IdentityUser user, bool definirSenha = false)
        {
            var linkConfirmacao = await GerarLinkConfirmacao(user, definirSenha);

            var corpoEmail = $"Confirme sua conta clicando <a href='{linkConfirmacao}'>aqui</a>";

            await _emailService.Enviar(user.Email, "Parametriz - AutoNFP - Confirmação de Senha",
               corpoEmail);
        }

        private async Task<string> GerarLinkConfirmacao(IdentityUser user, bool definirSenha)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = $"{"url_do_site"}/confirmar-email?email={user.Email}&code={code}&definirSenha={definirSenha}";

            return link;
        }
    }
}
