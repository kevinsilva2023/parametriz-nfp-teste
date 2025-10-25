using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using Parametriz.AutoNFP.Domain.ErrosTransmissaoLote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Repository
{
    public class ErroTransmissaoLoteRepository : InstituicaoEntityRepository<ErroTransmissaoLote>, IErroTransmissaoLoteRepository
    {
        public ErroTransmissaoLoteRepository(AutoNfpDbContext context) 
            : base(context)
        {
        }

        public async override Task<bool> EhUnico(ErroTransmissaoLote erroTransmissaoLote)
        {
            return !await _context.ErrosTransmissaoLote
                .AnyAsync(e => e.InstituicaoId == erroTransmissaoLote.InstituicaoId &&
                               e.Id != erroTransmissaoLote.Id &&
                               e.VoluntarioId == erroTransmissaoLote.VoluntarioId &&
                               e.Mensagem == erroTransmissaoLote.Mensagem);
        }

        public bool Existe(Guid instituicaoId, Guid? voluntarioId, string mensagem)
        {
            return _context.ErrosTransmissaoLote
                .Any(e => e.InstituicaoId == instituicaoId &&
                          e.VoluntarioId == voluntarioId &&
                          e.Mensagem == mensagem);
        }

        public async Task<IEnumerable<ErroTransmissaoLote>> ObterPorInstituicaoId(Guid instituicaoId)
        {
            return await _context.ErrosTransmissaoLote
                .Include(p => p.Voluntario)
                .AsNoTrackingWithIdentityResolution()
                .Where(e => e.InstituicaoId == instituicaoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ErroTransmissaoLote>> ObterPorVoluntarioId(Guid voluntarioId, Guid instituicaoId)
        {
            return await _context.ErrosTransmissaoLote
                .Include(p => p.Voluntario)
                .AsNoTrackingWithIdentityResolution()
                .Where(e => e.InstituicaoId == instituicaoId &&
                            (e.VoluntarioId == null || e.VoluntarioId == voluntarioId))
                .ToListAsync();
        }

        public void ExcluirPorInstituicaoId(Guid instituicaoId)
        {
            var errosTransmissaoLote = _context.ErrosTransmissaoLote
                .AsNoTracking()
                .Where(e => e.InstituicaoId == instituicaoId)
                .ToList();

            _context.RemoveRange(errosTransmissaoLote);
        }
    }
}
