using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Application.JwtToken.Services;
using Parametriz.AutoNFP.Api.Application.Usuarios.Services;
using Parametriz.AutoNFP.Api.Configs;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Usuarios;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/identidade")]
    public class AuthController : MainController
    {
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IInstituicaoRepository _instituicaoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IInstituicaoService _instituicaoService;
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UrlsConfig _urlsConfig;

        public AuthController(Notificador notificador,
                              IAspNetUser user,
                              IJwtTokenService jwtTokenService,
                              IInstituicaoService instituicaoService,
                              IEmailService emailService,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IOptions<UrlsConfig> options,
                              IInstituicaoRepository instituicaoRepository,
                              IUsuarioRepository usuarioRepository,
                              IUsuarioService usuarioService)
            : base(notificador, user)
        {
            _jwtTokenService = jwtTokenService;
            _instituicaoService = instituicaoService;
            _emailService = emailService;
            _signInManager = signInManager;
            _userManager = userManager;
            _urlsConfig = options.Value;
            _instituicaoRepository = instituicaoRepository;
            _usuarioRepository = usuarioRepository;
            _usuarioService = usuarioService;
            _jwtTokenService = jwtTokenService;
        }

        [AllowAnonymous]
        [HttpPost("cadastrar-instituicao")]
        public async Task<IActionResult> Post([FromBody] CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(cadastrarInstituicaoViewModel);

            var user = new IdentityUser { UserName = cadastrarInstituicaoViewModel.Email, Email = cadastrarInstituicaoViewModel.Email };
            var result = await _userManager.CreateAsync(user, cadastrarInstituicaoViewModel.Senha);

            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return CustomResponse(cadastrarInstituicaoViewModel);
            }
                
            result = await _userManager.AddToRoleAsync(user, "Administrador");

            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return CustomResponse(cadastrarInstituicaoViewModel);
            }

            user = await _userManager.FindByEmailAsync(user.Email);

            if (!await _instituicaoService.Cadastrar(cadastrarInstituicaoViewModel, Guid.Parse(user.Id)))
            {
                await _userManager.DeleteAsync(user);
                return CustomResponse(cadastrarInstituicaoViewModel);
            }

            var instituicaoId = await _instituicaoRepository.ObterIdPorUsuarioId(Guid.Parse(user.Id));

            await IncluirClaimInstituicaoId(user, instituicaoId);

            await EnviarLinkConfirmarEmail(user, cadastrarInstituicaoViewModel.UsuarioNome);

            return CustomResponse(cadastrarInstituicaoViewModel);
        }

        private async Task IncluirClaimInstituicaoId(IdentityUser user, Guid instituicaoId)
        {
            await _userManager.AddClaimAsync(user, new Claim("instituicaoId", instituicaoId.ToString()));
        }

        private async Task EnviarLinkConfirmarEmail(IdentityUser user, string usuarioNome, bool definirSenha = false)
        {
            var linkConfirmacao = await GerarLinkConfirmacao(user, definirSenha);

            var anexos = GerarAnexosEmail();

            var corpoEmail = GerarCorpoEmailConfirmacao(usuarioNome.Trim().ToUpper(), linkConfirmacao);

            await _emailService.Enviar(user.Email, "Confirmação de E-mail - AutoNFP - Parametriz",
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

        private string GerarCorpoEmailConfirmacao(string usuarioNome, string linkConfirmacao)
        {
            var body =
                $@"<table align=""center"" width=""80%"" style=""background-color: #fffaf5; border: 2px solid #003366; max-width: 800px;"">" +
                    $@"<tr>" +
                        $@"<td style=""background-color: #003366; text-align: center; padding: 20px;"">" +
                            $@"<a href=""https://www.parametriz.com.br"" target=""_blank"">" +
                                $@"<img src=""cid:logoWhiteId"" width=""60%"">" +
                            $@"</a>" +
                        $@"</td>" +
                    $@"</tr>" +
                    $@"<tr>" +
                        $@"<td style=""padding: 30px; text-align: center; color: #333333;"">" +
                            $@"<h1 style=""color: #003366; text-align: center; margin-bottom: 20px;"">Confirme seu email para ativar o <br> AUTONFP!</h1>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Olá {usuarioNome},</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Obrigado por se registrar na Parametriz AutoNFP! Para ativar sua conta e começar a aproveitar todos os nossos serviços, por favor, confirme seu endereço de email clicando no botão abaixo:</p>" +
                            $@"<table align=""center"" cellpadding=""0"" cellspacing=""0"" border=""0"">" +
                                $@"<tr>" +
                                    $@"<td bgcolor=""#003366"" style=""padding: 10px; text-align: center;"">" +
                                        $@"<a href=""{linkConfirmacao}"" style=""color: #ffffff; font-weight: bold; font-size: 20px; text-decoration: none; display: block;"">Confirmar Email</a>" +
                                    $@"</td>" +
                                $@"</tr>" +
                            $@"</table>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Se o botão acima não funcionar, clicar no link abaixo:</p>" +
                            $@"<table width=""80%"" align=""center"" cellpadding=""0"" cellspacing=""0"" border=""0"">" +
                                $@"<tr>" +
                                    $@"<td style=""word-wrap: break-word; word-break: break-all; text-align:center"">" +
                                        $@"<a href=""{linkConfirmacao}"" style=""color: #007bff; text-decoration: underline;"">{linkConfirmacao}</a>" +
                                    $@"</td>" +
                                $@"</tr>" +
                            $@"</table>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Este link de confirmação é válido por 24 horas. Se você não se registrou na Parametriz, por favor, ignore este email.</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Atenciosamente,</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 0;"">A Equipe Parametriz</p>" +
                        $@"</td>" +
                    $@"</tr>" +
                    $@"<tr>" +
                        $@"<td style=""background-color: #f9f9f9; text-align: center; padding: 20px; font-size: 12px; color: #777777; border-top: 1px solid #e0e0e0;"">" +
                            $@"<p style=""margin:0;"">&copy; {DateTime.Now.Year} Parametriz Soluções Tecnológicas. Todos os direitos reservados.</p>" +
                            $@"<p style=""margin: 5px 0; color: #003366;"">Política de Privacidade | Termos de Serviço</p>" +
                            $@"<a href=""https://www.parametriz.com.br"" target=""_blank"">" +
                                $@"<img src=""cid:logoCircleId"" width=""10%"">" +
                            $@"</a>" +
                        $@"</td>" +
                    $@"</tr>" +
                $@"</table>";

            return body;
        }

        [HttpPost("enviar-confirmar-email")]
        public async Task<IActionResult> EnviarConfirmacaoEmail([FromBody] EnviarConfirmarEmailViewModel enviarConfirmarEmailViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(enviarConfirmarEmailViewModel);
            
            var user = await _userManager.FindByIdAsync(enviarConfirmarEmailViewModel.UsuarioId.ToString());
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            if (user.EmailConfirmed)
            {
                NotificarErro("Conta de e-mail já confirmada.");
                return CustomResponse();
            }

            var instituicao = await _instituicaoRepository.ObterPorUsuarioId(enviarConfirmarEmailViewModel.UsuarioId);
            var usuario = await _usuarioRepository.ObterPorId(enviarConfirmarEmailViewModel.UsuarioId, instituicao.Id);

            if (InstituicaoId != instituicao.Id)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            await EnviarLinkConfirmarEmail(user, usuario.Nome, enviarConfirmarEmailViewModel.DefinirSenha);
            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("confirmar-email")]
        public async Task<IActionResult> ConfirmarEmail([FromBody] ConfirmarEmailViewModel confirmarEmailViewModel)
        {
            if (!ModelStateValida())
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse(confirmarEmailViewModel);
            }

            var user = await _userManager.FindByEmailAsync(confirmarEmailViewModel.Email);
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            var instituicao = await _instituicaoRepository.ObterPorUsuarioId(Guid.Parse(user.Id));
            var usuario = await _usuarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmarEmailViewModel.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                if (confirmarEmailViewModel.DefinirSenha)
                    await EnviarLinkDefinirSenha(user, usuario.Nome);

                return CustomResponse();
            }

            AdicionarErrosIdentity(result);
            return CustomResponse();
        }

        private async Task EnviarLinkDefinirSenha(IdentityUser user, string usuarioNome)
        {
            var linkDefinirSenha = await GerarLinkDefinirSenha(user);

            var anexos = GerarAnexosEmail();

            var corpoEmail = GerarCorpoEmailDefinirSenha(usuarioNome, linkDefinirSenha);

            await _emailService.Enviar(user.Email, "Definir Senha - AutoNFP - Parametriz",
               corpoEmail, anexos);
        }

        private async Task<string> GerarLinkDefinirSenha(IdentityUser user)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var link = $"{_urlsConfig.Angular}/definir-senha?email={user.Email}&code={code}";

            return link;
        }

        private string GerarCorpoEmailDefinirSenha(string usuarioNome, string linkDefinirSenha)
        {
            var body = 
                $@"<table align=""center"" width=""80%"" style=""background-color: #fffaf5; border: 2px solid #003366; max-width:  800px;"">" +
                    $@"<tr>" +
                        $@"<td style=""background-color: #003366; text-align: center; padding: 20px;"">" +
                            $@"<a href=""https://www.parametriz.com.br"" target=""_blank"">" +
                                $@"<img src=""cid:logoWhiteId"" width=""60%"">" +
                            $@"</a>" +
                        $@"</td>" +
                    $@"</tr>" +
                    $@"<tr>" +
                        $@"<td style=""padding: 30px; text-align: center; color: #333333;"">" +
                            $@"<h1 style=""color: #003366; text-align: center; margin-bottom: 20px;"">Solicitação de definição de senha</h1>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Olá {usuarioNome},</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Recebemos uma solicitação para redefinir a senha da sua conta Parametriz. Se você não solicitou esta alteração, por favor, ignore este email.<br><br> Para redefinir sua senha, clique no botão abaixo:</p>" +
                            $@"<table align=""center"" cellpadding=""0"" cellspacing=""0"" border=""0"">" +
                                $@"<tr>" +
                                    $@"<td bgcolor=""#003366"" style=""padding: 10px; text-align: center;"">" +
                                        $@"<a href=""{linkDefinirSenha}"" style=""color: #ffffff; font-weight: bold; font-size: 20px; text-decoration: none; display: block;"">Definir Senha</a>" +
                                    $@"</td>" +
                                $@"</tr>" +
                            $@"</table>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Se o botão acima não funcionar, clicar no link abaixo:</p>" +
                            $@"<table width=""80%"" align=""center"" cellpadding=""0"" cellspacing=""0"" border=""0"">" +
                                $@"<tr>" +
                                    $@"<td style=""word-wrap: break-word; word-break: break-all; text-align:center"">" +
                                        $@"<a href=""{linkDefinirSenha}"" style=""color: #007bff; text-decoration: underline;"">{linkDefinirSenha}</a>" +
                                    $@"</td>" +
                                $@"</tr>" +
                            $@"</table>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Este link de confirmação é válido por 24 horas. Se você não se registrou na Parametriz, por favor, ignore este email.</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 15px;"">Atenciosamente,</p>" +
                            $@"<p style=""font-size: 16px; line-height: 1.6; margin-bottom: 0;"">A Equipe Parametriz</p>" +
                        $@"</td>" +
                    $@"</tr>" +
                    $@"<tr>" +
                        $@"<td style=""background-color: #f9f9f9; text-align: center; padding: 20px; font-size: 12px; color: #777777; border-top: 1px solid #e0e0e0;"">" +
                            $@"<p style=""margin: 0;"">&copy; {DateTime.Now.Year} Parametriz Soluções Tecnológicas. Todos os direitos reservados.</p>" +
                            $@"<p style=""margin: 5px 0; color: #003366;"">Política de Privacidade | Termos de Serviço</p>" +
                            $@"<a href=""https://www.parametriz.com.br"" target=""_blank"">" +
                                $@"<img src=""cid:logoCircleId"" width=""10%"">" +
                            $@"</a>" +
                        $@"</td>" +
                    $@"</tr>" +
                $@"</table>";
                
            return body;
        }

        [AllowAnonymous]
        [HttpPost("enviar-definir-senha")]
        public async Task<IActionResult> EnviarDefinirSenha([FromBody] EnviarDefinirSenhaViewModel enviarDefinirSenhaViewModel)
        {
            if (!ModelStateValida())
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse(enviarDefinirSenhaViewModel);
            }

            var user = await _userManager.FindByEmailAsync(enviarDefinirSenhaViewModel.Email);
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            if (!user.EmailConfirmed)
            {
                NotificarErro("E-mail não confirmado.");
                return CustomResponse();
            }

            var instituicao = await _instituicaoRepository.ObterPorUsuarioId(Guid.Parse(user.Id));
            var usuario = await _usuarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            await EnviarLinkDefinirSenha(user, usuario.Nome);
            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("definir-senha")]
        public async Task<IActionResult> DefinirSenha([FromBody] DefinirSenhaViewModel definirSenhaViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(definirSenhaViewModel);

            var user = await _userManager.FindByEmailAsync(definirSenhaViewModel.Email);
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            var instituicao = await _instituicaoRepository.ObterPorUsuarioId(Guid.Parse(user.Id));
            var usuario = await _usuarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return CustomResponse();
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(definirSenhaViewModel.Code));

            var result = await _userManager.ResetPasswordAsync(user, code, definirSenhaViewModel.Senha);
            if (result.Succeeded)
                return CustomResponse(await _jwtTokenService.ObterLoginResponse(instituicao, usuario));

            AdicionarErrosIdentity(result);
            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                NotificarErro("Falha ao realizar o login");
                return CustomResponse(loginViewModel);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                NotificarErro("Verifique sua conta de e-mail e confirme seu cadastro, após a confirmação seu login será liberado.");
                return CustomResponse(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Senha, false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                var instituicao = await _instituicaoRepository.ObterPorUsuarioId(Guid.Parse(user.Id));
                var usuario = await _usuarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

                if (instituicao.Desativada)
                {
                    NotificarErro("Instituição desativada, favor entrar em contato com o suporte.");
                    return CustomResponse(loginViewModel);
                }

                if (usuario.Desativado)
                {
                    NotificarErro("Usuário desativado, favor entrar em contato com o administrador do sistema na instituição.");
                    return CustomResponse(loginViewModel);
                }

                return CustomResponse(await _jwtTokenService.ObterLoginResponse(instituicao, usuario));
            }

            NotificarErro("Falha ao realizar o login");
            return CustomResponse(loginViewModel);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] Guid refreshToken)
        {
            if (refreshToken == Guid.Empty)
            {
                NotificarErro("Refresh Token inválido.");
                return CustomResponse();
            }

            var token = await _jwtTokenService.ObterRefreshToken(refreshToken);

            if (token is null)
            {
                NotificarErro("Refresh Token expirado");
                return CustomResponse();
            }

            var instituicao = await _instituicaoRepository.ObterPorId(token.InstituicaoId);
            var usuario = await _usuarioRepository.ObterPorId(token.UsuarioId, token.InstituicaoId);

            return CustomResponse(await _jwtTokenService.ObterLoginResponse(instituicao, usuario, token));
        }

        [HttpPost("cadastrar-voluntario")]
        public async Task<IActionResult> Post([FromBody] CadastrarUsuarioViewModel cadastrarUsuarioViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(cadastrarUsuarioViewModel);

            var user = new IdentityUser { UserName = cadastrarUsuarioViewModel.Email, Email = cadastrarUsuarioViewModel.Email };
            var resultUser = await _userManager.CreateAsync(user);

            if (resultUser.Succeeded)
            {
                if (!await _usuarioService.Cadastrar(cadastrarUsuarioViewModel))
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(cadastrarUsuarioViewModel);
                }

                await IncluirClaimInstituicaoId(user, cadastrarUsuarioViewModel.InstituicaoId);

                await EnviarLinkConfirmarEmail(user, cadastrarUsuarioViewModel.Nome.Trim().ToUpper(), definirSenha: true);
                return CustomResponse(cadastrarUsuarioViewModel);
            }
            AdicionarErrosIdentity(resultUser);

            return CustomResponse(cadastrarUsuarioViewModel);
        }
    }
}
