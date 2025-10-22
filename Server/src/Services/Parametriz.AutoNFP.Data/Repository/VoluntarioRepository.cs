using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.Usuarios;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Repository
{
    public class VoluntarioRepository : InstituicaoEntityRepository<Voluntario>, IVoluntarioRepository
    {
        public VoluntarioRepository(AutoNfpDbContext context) 
            : base(context)
        {
        }

        public override async Task<bool> EhUnico(Voluntario usuario)
        {
            return  !await _context.Voluntarios
                .AnyAsync(u => u.InstituicaoId == usuario.InstituicaoId &&
                               u.Nome == usuario.Nome &&
                               u.Id != usuario.Id);
        }

        public async Task<bool> ExistemOutrosVoluntariosNaInstituicao(Guid id, Guid instituicaoId)
        {
            return await _context.Voluntarios
                .AnyAsync(u => u.InstituicaoId == instituicaoId &&
                               u.Id != id);
        }

        public async Task<bool> ExistemOutrosAdministradoresNaInstituicao(Guid id, Guid instituicaoId)
        {
            return await _context.Voluntarios
                .AnyAsync(u => u.InstituicaoId == instituicaoId &&
                               u.Id != id &&
                               u.Administrador);    
        }

        public override async Task<Voluntario> ObterPorId(Guid id, Guid instituicaoId)
        {
            return await _context.Voluntarios
                .Include(p => p.Certificado)
                .AsNoTracking()
                .SingleOrDefaultAsync(v => v.InstituicaoId == instituicaoId &&
                                           v.Id == id);            
        }

        public async Task<IEnumerable<Voluntario>> ObterPorFiltros(Guid instituicaoId, string nome = "", string email = "",
            BoolTresEstados administrador = BoolTresEstados.Ambos, BoolTresEstados desativado = BoolTresEstados.Falso)
        {
            return await _context.Voluntarios
                .AsNoTracking()
                .Where(u => u.InstituicaoId == instituicaoId &&
                            u.Nome.ToUpper().Contains(nome.Trim().ToUpper()) &&
                            u.Email.Conta.ToUpper().Contains(email.Trim().ToUpper()) &&
                            (administrador == BoolTresEstados.Ambos || (u.Administrador == (administrador == BoolTresEstados.Verdadeiro))) &&
                            (desativado == BoolTresEstados.Ambos || (u.Desativado == (desativado == BoolTresEstados.Verdadeiro))))
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voluntario>> ObterAtivos(Guid instituicaoId)
        {
            return await _context.Voluntarios
                .AsNoTracking()
                .Where(u => u.InstituicaoId == instituicaoId &&
                            !u.Desativado)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }
    }
}
