using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
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

        public async Task<CupomFiscalPaginacao> ObterPorFiltrosPaginado(Guid instituicaoId, DateTime cadastradoEm, 
            DateTime? emitidoEm = null, Guid? cadastradoPorId = null, CupomFiscalStatus? status = null, int pagina = 1, 
            int registrosPorPagina = 15)
        {
            var cuponsFiscais = _context.CuponsFiscais
                .Include(p => p.ChaveDeAcesso)
                    .ThenInclude(p => p.Cnpj)
                .Include(p => p.CadastradoPor)
                .AsNoTracking()
                .Where(p => p.InstituicaoId == instituicaoId &&
                            p.CadastradoEm.Year == cadastradoEm.Year &&
                            p.CadastradoEm.Month == cadastradoEm.Month &&
                            (emitidoEm == null || 
                             (p.ChaveDeAcesso.EmitidoEm.Value.Month == emitidoEm.Value.Month &&
                              p.ChaveDeAcesso.EmitidoEm.Value.Year == emitidoEm.Value.Year)) &&
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

        public IEnumerable<Instituicao> ObterInstituicoesComCuponsFiscaisProcessando()
        {
            return _context.CuponsFiscais
                .Include(p => p.Instituicao)
                .AsNoTrackingWithIdentityResolution()
                .Where(c => c.Status == CupomFiscalStatus.PROCESSANDO)
                .Select(c => c.Instituicao)
                .Distinct()
                .ToList();
        }

        public IEnumerable<Voluntario> ObterVoluntariosComCuponsFiscaisProcessando(Guid instituicaoId)
        {
            return _context.CuponsFiscais
                .Include(p => p.CadastradoPor)
                .AsNoTrackingWithIdentityResolution()
                .Where(c => c.Status == CupomFiscalStatus.PROCESSANDO)
                .Select(c => c.CadastradoPor)
                .Distinct()
                .ToList();
        }

        public IEnumerable<CupomFiscal> ObterPorStatusProcessando(Guid voluntarioId, Guid instituicaoId)
        {
            return _context.CuponsFiscais
                .AsNoTracking()
                .Where(c => c.InstituicaoId == instituicaoId &&
                            c.CadastradoPorId == voluntarioId &&
                            c.Status == CupomFiscalStatus.PROCESSANDO)
                .ToList();
        }
    }
}
