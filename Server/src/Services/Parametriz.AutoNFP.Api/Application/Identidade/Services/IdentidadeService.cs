using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Parametriz.AutoNFP.Api.Application.Email.Services;
using Parametriz.AutoNFP.Api.Application.Instituicoes.Services;
using Parametriz.AutoNFP.Api.Application.JwtToken.Services;
using Parametriz.AutoNFP.Api.Application.Voluntarios.Services;
using Parametriz.AutoNFP.Api.Configs;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Usuarios;
using System.Drawing;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace Parametriz.AutoNFP.Api.Application.Identidade.Services
{
    public class IdentidadeService : BaseService, IIdentidadeService
    {
        private readonly IInstituicaoRepository _instituicaoRepository;
        private readonly IVoluntarioRepository _voluntarioRepository;
        private readonly IInstituicaoService _instituicaoService;
        private readonly IVoluntarioService _voluntarioService;
        private readonly IEmailService _emailService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UrlsConfig _urlsConfig;

        public IdentidadeService(IAspNetUser user,
                                 IUnitOfWork uow,
                                 Notificador notificador,
                                 IInstituicaoRepository instituicaoRepository,
                                 IVoluntarioRepository voluntarioRepository,
                                 IInstituicaoService instituicaoService,
                                 IVoluntarioService voluntarioService,
                                 IEmailService emailService,
                                 IJwtTokenService jwtTokenService,
                                 SignInManager<IdentityUser> signInManager,
                                 UserManager<IdentityUser> userManager,
                                 IOptions<UrlsConfig> options)
            : base(user, uow, notificador)
        {
            _instituicaoRepository = instituicaoRepository;
            _voluntarioRepository = voluntarioRepository;
            _instituicaoService = instituicaoService;
            _voluntarioService = voluntarioService;
            _emailService = emailService;
            _jwtTokenService = jwtTokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _urlsConfig = options.Value;
        }

        #region Identity
        private async Task<IdentityUser> ObterIdentityUserPorEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        private async Task<bool> CadastrarIdentityUser(IdentityUser user)
        {
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return false;
            }

            return true;
        }

        private async Task<bool> CadastrarRoleNoUsuario(IdentityUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);

            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return false;
            }

            return true;
        }

        private async Task<bool> RemoverRoleDoUsuario(IdentityUser user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);

            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return false;
            }

            return true;
        }

        private async Task<bool> CadastrarClaimNoUsuario(IdentityUser user, string claimType, string claimValue)
        {
            var result = await _userManager.AddClaimAsync(user, new Claim(claimType, claimValue));

            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return false;
            }
            return true;
        }
        #endregion Identity

        #region Email
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

        #region EmailConfirmacao
        private async Task EnviarLinkConfirmarEmail(IdentityUser user, string usuarioNome, bool definirSenha)
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
        #endregion EmailConfirmacao

        #region EmailDefinirSenha
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
        #endregion EmailDefinirSenha

        #endregion Email

        public async Task<bool> CadastrarInstituicao(CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel)
        {
            var user = new IdentityUser { UserName = cadastrarInstituicaoViewModel.Email, Email = cadastrarInstituicaoViewModel.Email };
            
            if (!await CadastrarIdentityUser(user)) 
                return false;

            //user = await ObterIdentityUserPorEmail(user.Email);

            if (!await CadastrarRoleNoUsuario(user, "Administrador"))
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            if (!await CadastrarClaimNoUsuario(user, "instituicaoId", cadastrarInstituicaoViewModel.Id.ToString()))
            {
                await _userManager.DeleteAsync(user);
                return false;
            }
            
            if (!await _instituicaoService.Cadastrar(cadastrarInstituicaoViewModel, Guid.Parse(user.Id)))
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            await EnviarLinkConfirmarEmail(user, cadastrarInstituicaoViewModel.VoluntarioNome, definirSenha: true);

            return true;
        }

        public async Task EnviarConfirmarEmail(EnviarConfirmarEmailViewModel enviarConfirmarEmailViewModel)
        {
            var user = await _userManager.FindByIdAsync(enviarConfirmarEmailViewModel.UsuarioId.ToString());
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return;
            }

            if (user.EmailConfirmed)
            {
                NotificarErro("Conta de e-mail já confirmada.");
                return;
            }

            var instituicao = await _instituicaoRepository.ObterPorVoluntarioId(enviarConfirmarEmailViewModel.UsuarioId);
            var usuario = await _voluntarioRepository.ObterPorId(enviarConfirmarEmailViewModel.UsuarioId, instituicao.Id);

            if (InstituicaoId != instituicao.Id)
            {
                NotificarErro("Requisição inválida.");
                return;
            }

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return;
            }

            if (enviarConfirmarEmailViewModel.DefinirSenha)
                await EnviarLinkConfirmarEmail(user, usuario.Nome, enviarConfirmarEmailViewModel.DefinirSenha);
        }

        public async Task ConfirmarEmail(ConfirmarEmailViewModel confirmarEmailViewModel)
        {
            var user = await _userManager.FindByEmailAsync(confirmarEmailViewModel.Email);
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return;
            }

            var instituicao = await _instituicaoRepository.ObterPorVoluntarioId(Guid.Parse(user.Id));
            var usuario = await _voluntarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return;
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(confirmarEmailViewModel.Code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return;
            }

            if (confirmarEmailViewModel.DefinirSenha)
                await EnviarLinkDefinirSenha(user, usuario.Nome);
        }

        public async Task EnviarDefinirSenha(EnviarDefinirSenhaViewModel enviarDefinirSenhaViewModel)
        {
            var user = await _userManager.FindByEmailAsync(enviarDefinirSenhaViewModel.Email);
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return;
            }

            if (!user.EmailConfirmed)
            {
                NotificarErro("E-mail não confirmado.");
                return;
            }

            var instituicao = await _instituicaoRepository.ObterPorVoluntarioId(Guid.Parse(user.Id));
            var usuario = await _voluntarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return;
            }

            await EnviarLinkDefinirSenha(user, usuario.Nome);
        }

        public async Task<LoginResponseViewModel> DefinirSenha(DefinirSenhaViewModel definirSenhaViewModel)
        {
            var user = await _userManager.FindByEmailAsync(definirSenhaViewModel.Email);
            if (user == null)
            {
                NotificarErro("Requisição inválida.");
                return null;
            }

            var instituicao = await _instituicaoRepository.ObterPorVoluntarioId(Guid.Parse(user.Id));
            var usuario = await _voluntarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

            if (instituicao.Desativada || usuario.Desativado)
            {
                NotificarErro("Requisição inválida.");
                return null;
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(definirSenhaViewModel.Code));

            var result = await _userManager.ResetPasswordAsync(user, code, definirSenhaViewModel.Senha);
            
            if (!result.Succeeded)
            {
                AdicionarErrosIdentity(result);
                return null;
            }

            return await _jwtTokenService.ObterLoginResponse(instituicao, usuario);
        }

        public async Task<LoginResponseViewModel> Login(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                NotificarErro("Falha ao realizar o login");
                return null;
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                NotificarErro("Verifique sua conta de e-mail e confirme seu cadastro, após a confirmação seu login será liberado.");
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Senha, false, lockoutOnFailure: true);

            if (!result.Succeeded)
            {
                NotificarErro("Falha ao realizar o login");
                return null;
            }

            if (await _userManager.IsInRoleAsync(user, "Parametriz"))
            {
                return await _jwtTokenService.ObterParametrizLoginReponse(user);
            }

            var instituicao = await _instituicaoRepository.ObterPorVoluntarioId(Guid.Parse(user.Id));
            var usuario = await _voluntarioRepository.ObterPorId(Guid.Parse(user.Id), instituicao.Id);

            if (instituicao.Desativada)
            {
                NotificarErro("Instituição desativada, favor entrar em contato com o suporte.");
                return null;
            }

            if (usuario.Desativado)
            {
                NotificarErro("Usuário desativado, favor entrar em contato com o administrador do sistema na instituição.");
                return null;
            }

            return await _jwtTokenService.ObterLoginResponse(instituicao, usuario);
        }

        public async Task<LoginResponseViewModel> UtilizarRefreshToken(Guid refreshToken)
        {
            var token = await _jwtTokenService.ObterRefreshToken(refreshToken);

            if (token is null)
            {
                NotificarErro("Refresh Token expirado");
                return null;
            }

            var instituicao = await _instituicaoRepository.ObterPorId(token.InstituicaoId);
            var usuario = await _voluntarioRepository.ObterPorId(token.UsuarioId, token.InstituicaoId);

            return await _jwtTokenService.ObterLoginResponse(instituicao, usuario, token);
        }


        #region Usuarios
        public async Task<bool> CadastrarVoluntario(VoluntarioViewModel voluntarioViewModel)
        {
            var user = new IdentityUser { UserName = voluntarioViewModel.Email, Email = voluntarioViewModel.Email };

            if (!await CadastrarIdentityUser(user))
                return false;

            //user = await ObterIdentityUserPorEmail(user.Email);

            if (voluntarioViewModel.Administrador)
            {
                if (!await CadastrarRoleNoUsuario(user, "Administrador"))
                    return false;
            }

            if (!await CadastrarClaimNoUsuario(user, "instituicaoId", InstituicaoId.ToString()))
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            if (!await _voluntarioService.Cadastrar(voluntarioViewModel, Guid.Parse(user.Id)))
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            await EnviarLinkConfirmarEmail(user, voluntarioViewModel.Nome.Trim().ToUpper(), definirSenha: true);

            return true;
        }
        #endregion Usuarios
    }
}
