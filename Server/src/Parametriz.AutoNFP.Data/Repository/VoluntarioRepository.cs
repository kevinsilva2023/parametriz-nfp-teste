using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
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

        public async Task<bool> ExisteNaInstituicao(Guid instituicaoId)
        {
            return await _context.Voluntarios
                .AnyAsync(v => v.InstituicaoId == instituicaoId);
        }

        public override async Task<bool> EhUnico(Voluntario voluntario)
        {
            return !await _context.Voluntarios
                .AnyAsync(v => v.InstituicaoId == voluntario.InstituicaoId &&
                               v.Id != voluntario.Id);
        }

        public async Task<Voluntario> ObterPorInstituicaoId(Guid instituicaoId)
        {
            return await _context.Voluntarios
                .AsNoTracking()
                .SingleOrDefaultAsync(v => v.InstituicaoId == instituicaoId);
        }

        public async Task Excluir(Guid instituicaoId)
        {
            var voluntario = await _context.Voluntarios
                .AsNoTracking()
                .SingleOrDefaultAsync(v => v.InstituicaoId == instituicaoId);

            _context.Voluntarios.Remove(voluntario);
        }
    }
}
