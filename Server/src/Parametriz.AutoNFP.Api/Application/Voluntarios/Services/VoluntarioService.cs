using Parametriz.AutoNFP.Api.Models.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Application.Voluntarios.Services
{
    public class VoluntarioService : BaseService, IVoluntarioService
    {
        private readonly IVoluntarioRepository _voluntarioRepository;

        public VoluntarioService(IAspNetUser user,
                                 IUnitOfWork uow,
                                 Notificador notificador,
                                 IVoluntarioRepository voluntarioRepository)
            : base(user, uow, notificador)
        {
            _voluntarioRepository = voluntarioRepository;
        }

        private async Task ValidarVoluntario(Voluntario voluntario)
        {
            await ValidarEntidade(new VoluntarioValidation(), voluntario);
        }

        private async Task VoluntarioEhUnico(Voluntario voluntario)
        {
            if (!await _voluntarioRepository.EhUnico(voluntario))
                _notificador.IncluirNotificacao("Voluntário já cadastrado.");
        }

        private async Task<bool> VoluntarioAptoParaCadastrar(Voluntario voluntario)
        {
            ValidarInstituicao(voluntario.InstituicaoId);
            await ValidarVoluntario(voluntario);
            await VoluntarioEhUnico(voluntario);

            return CommandEhValido();
        }

        public async Task<bool> Cadastrar(CadastrarVoluntarioViewModel cadastrarVoluntarioViewModel)
        {
            var voluntario = new Voluntario(Guid.NewGuid(), cadastrarVoluntarioViewModel.InstituicaoId, 
                cadastrarVoluntarioViewModel.Nome, new Domain.Core.ValueObjects.Email(cadastrarVoluntarioViewModel.Email));

            if (!await VoluntarioAptoParaCadastrar(voluntario))
                return false;

            await _voluntarioRepository.Cadastrar(voluntario);

            await Commit();

            return CommandEhValido();
        }
    }
}
