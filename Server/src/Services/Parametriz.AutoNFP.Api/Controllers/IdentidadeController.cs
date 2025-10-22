using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Application.Identidade.Services;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Core.Notificacoes;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Route("api/identidade")]
    public class IdentidadeController : MainController
    {
        private readonly IIdentidadeService _identidadeService;

        public IdentidadeController(Notificador notificador,
                                    IAspNetUser user,
                                    IIdentidadeService identidadeService)
            : base(notificador, user)
        {
            _identidadeService = identidadeService;
        }

        [Authorize(Roles = "Parametriz")]
        [HttpPost("cadastrar-instituicao")]
        public async Task<IActionResult> Post([FromBody] CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(cadastrarInstituicaoViewModel);

            await _identidadeService.CadastrarInstituicao(cadastrarInstituicaoViewModel);
            
            return CustomResponse(cadastrarInstituicaoViewModel);
        }

        [HttpPost("enviar-confirmar-email")]
        public async Task<IActionResult> EnviarConfirmacaoEmail([FromBody] EnviarConfirmarEmailViewModel enviarConfirmarEmailViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(enviarConfirmarEmailViewModel);
            
            await _identidadeService.EnviarConfirmarEmail(enviarConfirmarEmailViewModel);
            
            return CustomResponse(enviarConfirmarEmailViewModel);
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

            await _identidadeService.ConfirmarEmail(confirmarEmailViewModel);

            return CustomResponse();
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

            await _identidadeService.EnviarDefinirSenha(enviarDefinirSenhaViewModel);

            return CustomResponse();
        }

        [AllowAnonymous]
        [HttpPost("definir-senha")]
        public async Task<IActionResult> DefinirSenha([FromBody] DefinirSenhaViewModel definirSenhaViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(definirSenhaViewModel);

            var loginResponseViewModel = await _identidadeService.DefinirSenha(definirSenhaViewModel);

            return CustomResponse(loginResponseViewModel);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelStateValida())
                return CustomResponse(loginViewModel);

            var loginResponseViewModel = await _identidadeService.Login(loginViewModel);

            return CustomResponse(loginResponseViewModel);
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

            var loginResponseViewModel = await _identidadeService.UtilizarRefreshToken(refreshToken);

            return CustomResponse(loginResponseViewModel);
        }
    }
}
