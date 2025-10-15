using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Core.Enums;
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

        public async Task<CupomFiscalPaginacao> ObterPorFiltrosPaginado(Guid instituicaoId, DateTime competencia, 
            Guid? cadastradoPorId = null, CupomFiscalStatus? status = null, int pagina = 1, int registrosPorPagina = 50)
        {
            var cuponsFiscais = _context.CuponsFiscais
                .Include(p => p.ChaveDeAcesso)
                    .ThenInclude(p => p.Cnpj)
                .Include(p => p.CadastradoPor)
                .AsNoTracking()
                .Where(p => p.InstituicaoId == instituicaoId &&
                            p.ChaveDeAcesso.Competencia.Value.Month == competencia.Month &&
                            p.ChaveDeAcesso.Competencia.Value.Year == competencia.Year &&
                            (cadastradoPorId == null || p.CadastradoPorId == cadastradoPorId) &&
                            (status == null || p.Status == status))
                .OrderByDescending(p => p.CadastradoEm);

            return new CupomFiscalPaginacao(
                await cuponsFiscais
                    .Skip(registrosPorPagina * (pagina - 1))
                    .Take(registrosPorPagina)
                    .ToListAsync(),
                pagina,
                registrosPorPagina,
                cuponsFiscais.Count(p => p.Status == CupomFiscalStatus.PROCESSANDO),
                cuponsFiscais.Count(p => p.Status == CupomFiscalStatus.SUCESSO),
                cuponsFiscais.Count(p => p.Status == CupomFiscalStatus.ERRO),
                cuponsFiscais.Count());
        }
    }
}
