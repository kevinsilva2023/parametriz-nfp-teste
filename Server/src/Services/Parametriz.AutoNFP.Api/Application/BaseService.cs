using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Core.Notificacoes;

namespace Parametriz.AutoNFP.Api.Application
{
    public abstract class BaseService
    {
        protected readonly IAspNetUser _user;
        protected readonly IUnitOfWork _uow;
        protected readonly Notificador _notificador;

        protected Guid InstituicaoId => _user.ObterInstituicaoId();
        protected Guid UsuarioId => _user.ObterId();

        public BaseService(IAspNetUser user, 
                           IUnitOfWork uow, 
                           Notificador notificador)
        {
            _user = user;
            _uow = uow;
            _notificador = notificador;
        }

        protected async Task ValidarEntidade<TV, TE>(TV validacao, TE entidade) where TV : AbstractValidator<TE>
        {
            var validationResult = await validacao.ValidateAsync(entidade);

            if (validationResult.IsValid)
                return;

            NotificarErrosValidacao(validationResult);
        }

        private void NotificarErrosValidacao(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                _notificador.IncluirNotificacao(error.ErrorMessage);
            }
        }

        protected bool AdicionarErrosIdentity(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                NotificarErro(error.Description);
            }
            return false;
        }

        protected bool NotificarErro(string value)
        {
            _notificador.IncluirNotificacao(value);

            return false;
        }

        protected bool CommandEhValido()
        {
            return !_notificador.TemNotificacoes();
        }

        protected async virtual Task<bool> Commit()
        {
            if (!CommandEhValido())
                return false;

            if (await _uow.CommitAsync())
                return true;

            return NotificarErro("Ocorreu um erro ao salvar os dados no banco");
        }
    }
}
