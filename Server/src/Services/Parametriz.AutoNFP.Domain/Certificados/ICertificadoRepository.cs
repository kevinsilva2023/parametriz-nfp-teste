using Parametriz.AutoNFP.Core.Interfaces;

namespace Parametriz.AutoNFP.Domain.Certificados
{
    public interface ICertificadoRepository : IRepository<Certificado>
    {
        Task<bool> ExisteNoVoluntario(Guid instituicaoId);

        Certificado ObterPorVoluntarioId(Guid instituicaoId);
        Task<Certificado> ObterPorVoluntarioIdAsync(Guid instituicaoId);

        Task Excluir(Guid instituicaoId);
    }
}
