using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Repository
{
    public class CupomFiscalRepository : InstituicaoEntityRepository<CupomFiscal>, ICupomFiscalRepository
    {
        public CupomFiscalRepository(AutoNfpDbContext context) 
            : base(context)
        {
        }

        public override async Task<bool> EhUnico(CupomFiscal cupomFiscal)
        {
            return !await _context.CuponsFiscais
                .AnyAsync(c => c.ChaveDeAcesso.Chave == cupomFiscal.ChaveDeAcesso.Chave &&
                               c.Id != cupomFiscal.Id);
        }

        public async Task<IEnumerable<CupomFiscal>> ObterPorUsuarioId(Guid usuarioId, Guid instituicaoId)
        {
            return await _context.CuponsFiscais
                .Include(p => p.Instituicao)
                .Include(p => p.CadastradoPor)
                .AsNoTrackingWithIdentityResolution()
                .Where(c => c.InstituicaoId == instituicaoId &&
                            c.CadastradoPorId == usuarioId)
                .OrderBy(c => c.ChaveDeAcesso.MesEmissao)
                .ToListAsync();
        }
    }
}
