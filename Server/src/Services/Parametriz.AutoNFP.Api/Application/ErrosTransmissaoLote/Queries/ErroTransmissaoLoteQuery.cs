using Parametriz.AutoNFP.Api.Extensions;
using Parametriz.AutoNFP.Api.ViewModels.ErrosTransmissaoLote;
using Parametriz.AutoNFP.Domain.ErrosTransmissaoLote;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Application.ErrosTransmissaoLote.Queries
{
    public class ErroTransmissaoLoteQuery : IErroTransmissaoLoteQuery
    {
        private readonly IErroTransmissaoLoteRepository _erroTransmissaoLoteRepository;
        private readonly IVoluntarioRepository _voluntarioRepository;

        public ErroTransmissaoLoteQuery(IErroTransmissaoLoteRepository erroTransmissaoLoteRepository, 
                                        IVoluntarioRepository voluntarioRepository)
        {
            _erroTransmissaoLoteRepository = erroTransmissaoLoteRepository;
            _voluntarioRepository = voluntarioRepository;
        }

        public async Task<IEnumerable<ErroTransmissaoLoteViewModel>> ObterPorVoluntarioId(Guid voluntarioId, Guid instituicaoId)
        {
            var administrador = await _voluntarioRepository.EhAdministrador(voluntarioId, instituicaoId);

            if (administrador)
                return (await _erroTransmissaoLoteRepository.ObterPorInstituicaoIdAsync(instituicaoId)).ToViewModel();

            return (await _erroTransmissaoLoteRepository.ObterPorVoluntarioId(voluntarioId, instituicaoId)).ToViewModel();
        }
    }
}
