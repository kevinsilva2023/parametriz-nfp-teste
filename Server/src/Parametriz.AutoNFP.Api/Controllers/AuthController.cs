using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NetDevPack.Identity.Interfaces;
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using System.Text;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IInstituicaoService _instituicaoService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(Notificador notificador,
                              IAspNetUser user,
                              IJwtBuilder jwtBuilder,
                              IInstituicaoService instituicaoService,
                              IEmailService emailService,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager)
            : base(notificador, user)
        {
            _jwtBuilder = jwtBuilder;
            _instituicaoService = instituicaoService;
            _emailService = emailService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("cadastrar-instituicao")]
        public async Task<IActionResult> Post([FromBody] CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(cadastrarInstituicaoViewModel);

            var user = new IdentityUser { UserName = cadastrarInstituicaoViewModel.Email, Email = cadastrarInstituicaoViewModel.Email };
            var result = await _userManager.CreateAsync(user, cadastrarInstituicaoViewModel.Senha);

            if (result.Succeeded)
            {
                user = await _userManager.FindByEmailAsync(user.Email);

                if (!await _instituicaoService.Cadastrar(cadastrarInstituicaoViewModel, Guid.Parse(user.Id)))
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(cadastrarInstituicaoViewModel);
                }

                await EnviarLinkConfirmarEmail(user);
            }

            AdicionarErrosIdentity(result);

            return CustomResponse(cadastrarInstituicaoViewModel);
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
