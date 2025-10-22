using Parametriz.AutoNFP.Core.Interfaces;

namespace Parametriz.AutoNFP.Domain.Certificados
{
    public interface ICertificadoRepository : IRepository<Certificado>
    {
        Task<bool> ExisteNoVoluntario(Guid voluntarioId);

        Certificado ObterPorVoluntarioId(Guid voluntarioId);
        Task<Certificado> ObterPorVoluntarioIdAsync(Guid voluntarioId);

        Task Excluir(Guid voluntarioId);
    }
}
