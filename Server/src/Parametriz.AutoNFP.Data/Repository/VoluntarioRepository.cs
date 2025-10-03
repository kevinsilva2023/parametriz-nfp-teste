using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
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

        public override async Task<bool> EhUnico(Voluntario voluntario)
        {
            return  !await _context.Voluntarios
                .AnyAsync(u => u.InstituicaoId == voluntario.InstituicaoId &&
                               u.Nome == voluntario.Nome &&
                               u.Id != voluntario.Id);
        }
    }
}
