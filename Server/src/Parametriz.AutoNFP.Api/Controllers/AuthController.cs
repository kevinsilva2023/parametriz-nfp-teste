using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using NetDevPack.Identity.Interfaces;
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Configs;
using Parametriz.AutoNFP.Api.Data.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly IJwtBuilder _jwtBuilder;
        private readonly IInstituicaoRepository _instituicaoRepository;
        private readonly IInstituicaoService _instituicaoService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UrlsConfig _urlsConfig;

        public AuthController(Notificador notificador,
                              IAspNetUser user,
                              IJwtBuilder jwtBuilder,
                              IInstituicaoService instituicaoService,
                              IEmailService emailService,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<UrlsConfig> options,
                              IInstituicaoRepository instituicaoRepository)
            : base(notificador, user)
        {
            _jwtBuilder = jwtBuilder;
            _instituicaoService = instituicaoService;
            _emailService = emailService;
            _signInManager = signInManager;
            _userManager = userManager;
            _urlsConfig = options.Value;
            _instituicaoRepository = instituicaoRepository;
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

                await IncluirClaimInstituicaoId(user);

                await EnviarLinkConfirmarEmail(user, cadastrarInstituicaoViewModel.VoluntarioNome, definirSenha: true);
            }

            AdicionarErrosIdentity(result);

            return CustomResponse(cadastrarInstituicaoViewModel);
        }

        private async Task IncluirClaimInstituicaoId(IdentityUser user)
        {
            var instituicaoId = await _instituicaoRepository.ObterIdPorVoluntarioId(Guid.Parse(user.Id));

            await _userManager.AddClaimAsync(user, new Claim("instituicaoId", instituicaoId.ToString()));
        }

        private async Task EnviarLinkConfirmarEmail(IdentityUser user, string voluntarioNome, bool definirSenha = false)
        {
            var linkConfirmacao = await GerarLinkConfirmacao(user, definirSenha);

            var anexos = GerarAnexosEmail();

            var corpoEmail = GerarCorpoEmail(voluntarioNome.Trim().ToUpper(), linkConfirmacao);

            await _emailService.Enviar(user.Email, "Confirmação de Senha - AutoNFP - Parametriz",
               corpoEmail, anexos);
        }

        private async Task<string> GerarLinkConfirmacao(IdentityUser user, bool definirSenha)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = $"{_urlsConfig.Angular}/confirmar-email?email={user.Email}&code={code}&definirSenha={definirSenha}";

            return link;
        }

        private IEnumerable<Attachment> GerarAnexosEmail()
        {
            var anexos = new List<Attachment>();

            var logoWhite = new Attachment(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logos", "logo-white.png"));
            logoWhite.ContentId = "logoWhiteId";
            anexos.Add(logoWhite);

            var logoCircle = new Attachment(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logos", "logo-circle.png"));
            logoCircle.ContentId = "logoCircleId";
            anexos.Add(logoCircle);

            return anexos;
        }

        private string GerarCorpoEmail(string voluntarioNome, string linkConfirmacao)
        {
            var body =
                $@"<table align=""center"" width=""80%"" style=""background-color: #fffaf5; border: 2px solid #003366; max-width: 800px;"">" +
                    $@"<tr>" +
                        $@"<td style=""background-color: #003366; text-align: center; padding: 20px;"">" +
                            $@"<a href=""https://www.parametriz.com.br""><img src=""cid:logoWhiteId"" width=""60%""></a>" +
                        $@"</td>" +
                    $@"</tr>" +
                    $@"<tr>" +
                        $@"<td style=""padding: 30px; text-align: center; color: #333333;"">" +
                            $@"<h1 style=""color: #003366; text-align: center; margin-bottom: 20px;"">Confirme seu email para ativar o <br> AUTONFP!</h1>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Olá {voluntarioNome},</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Obrigado por se registrar na Parametriz AutoNFP! Para ativar sua conta e começar a aproveitar todos os nossos serviços, por favor, confirme seu endereço de email clicando no botão abaixo:</p>" +
                            $@"<table align=""center"" cellpadding=""0"" cellspacing=""0"" border=""0"">" +
                                $@"<tr>" +
                                    $@"<td bgcolor=""#003366"" style=""padding: 10px; text-align: center;"">" +
                                        $@"<a href=""{linkConfirmacao}"" style=""color: #ffffff; font-weight: bold; font-size: 20px; text-decoration: none; display: block;"">Confirmar Email</a>" +
                                    $@"</td>" +
                                $@"</tr>" +
                            $@"</table>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Se o botão acima não funcionar, clicar no link abaixo:</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;""><a href=""{linkConfirmacao}"" style=""color: #007bff; text-decoration: underline;"">{linkConfirmacao}</a></p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Este link de confirmação é válido por 24 horas. Se você não se registrou na Parametriz, por favor, ignore este email.</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Atenciosamente,</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 0;"">A Equipe Parametriz</p>" +
                        $@"</td>" +
                    $@"</tr>" +
                    $@"<tr>" +
                        $@"<td style=""background-color: #f9f9f9; text-align: center; padding: 20px; font-size: 12px; color: #777777; border-top: 1px solid #e0e0e0;"">" +
                            $@"<p style=""margin:0;"">&copy; {DateTime.Now.Year} Parametriz Soluções Tecnológicas. Todos os direitos reservados.</p>" +
                            $@"<p style=""margin: 5px 0; color: #003366;"">Política de Privacidade | Termos de Serviço</p>" +
                            $@"<a href=""https://www.parametriz.com.br""><img src=""cid:logoCircleId"" width=""10%""></a>" +
                        $@"</td>" +
                    $@"</tr>" +
                $@"</table>";

            return body;
        }
    }
}
