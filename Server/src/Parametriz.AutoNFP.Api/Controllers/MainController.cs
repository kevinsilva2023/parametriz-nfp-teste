using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;

namespace Parametriz.AutoNFP.Api.Controllers
{
    [Authorize]
    [ApiController]
    public abstract class MainController : Controller
    {
        private readonly Notificador _notificador;
        protected readonly IAspNetUser _user;

        protected Guid InstituicaoId => _user.ObterInstituicaoId();
        protected Guid VoluntarioId => _user.ObterId();

        protected MainController(Notificador notificador, 
                                 IAspNetUser user)
        {
            _notificador = notificador;
            _user = user;
        }

        protected IActionResult CustomResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "mensagens", _notificador.Notificacoes.ToArray() }
            }));
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacoes();
        }

        protected virtual bool ModelStateValida()
        {
            if (ModelState.IsValid)
                return true;

            NotificarErroModelInvalida();

            return false;
        }

        protected void NotificarErroModelInvalida()
        {
            var erros = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var erro in erros)
            {
                var erroMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                NotificarErro(erroMsg);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.IncluirNotificacao(mensagem);
        }

        protected void NotificarErros(List<string> mensagens)
        {
            foreach (var mensagem in mensagens)
            {
                NotificarErro(mensagem);
            }
        }

        protected void AdicionarErrosIdentity(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }
        }
    }
}
