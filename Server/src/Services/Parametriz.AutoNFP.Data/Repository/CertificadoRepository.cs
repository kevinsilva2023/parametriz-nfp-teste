using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.Certificados;

namespace Parametriz.AutoNFP.Data.Repository
{
    public class CertificadoRepository : Repository<Certificado>, ICertificadoRepository
    {
        public CertificadoRepository(AutoNfpDbContext context) 
            : base(context)
        {
        }

        public async Task<bool> ExisteNoVoluntario(Guid voluntarioId)
        {
            return await _context.Certificados
                .AnyAsync(v => v.VoluntarioId == voluntarioId);
        }

        public override async Task<bool> EhUnico(Certificado certificado)
        {
            return !await _context.Certificados
                .Include(p => p.Voluntario)
                .AnyAsync(c => c.Voluntario.InstituicaoId == certificado.Voluntario.InstituicaoId &&
                               c.Id != certificado.Id &&
                               c.Requerente == certificado.Requerente);
        }

        public Certificado ObterPorVoluntarioId(Guid voluntarioId)
        {
            return _context.Certificados
                .AsNoTracking()
                .SingleOrDefault(v => v.VoluntarioId == voluntarioId);
        }

        public async Task<Certificado> ObterPorVoluntarioIdAsync(Guid voluntarioId)
        {
            return await _context.Certificados
                .AsNoTracking()
                .SingleOrDefaultAsync(v => v.VoluntarioId == voluntarioId);
        }

        public async Task Excluir(Guid voluntarioId)
        {
            var voluntario = await _context.Certificados
                .AsNoTracking()
                .SingleOrDefaultAsync(v => v.VoluntarioId == voluntarioId);

            _context.Certificados.Remove(voluntario);
        }
    }
}
