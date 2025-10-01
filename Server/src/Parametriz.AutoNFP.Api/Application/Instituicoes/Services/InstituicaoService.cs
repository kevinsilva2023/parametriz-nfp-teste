using NetDevPack.Identity.User;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.Core.Notificacoes;
using Parametriz.AutoNFP.Domain.Instituicoes;

namespace Parametriz.AutoNFP.Api.Application.Instituicoes.Services
{
    public class InstituicaoService : BaseService, IInstituicaoService
    {
        private readonly IInstituicaoRepository _instituicaoRepository;

        public InstituicaoService(IAspNetUser user,
                                  IUnitOfWork uow,
                                  Notificador notificador,
                                  IInstituicaoRepository instituicaoRepository)
            : base(user, uow, notificador)
        {
            _instituicaoRepository = instituicaoRepository;
        }

        public Task<bool> Cadastrar(CadastrarInstituicaoViewModel cadastrarInstituicaoViewModel, Guid voluntarioId)
        {
            throw new NotImplementedException();
        }
    }
}
