using Parametriz.AutoNFP.Api.Models.User;
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
    }
}
