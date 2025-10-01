using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Data.Repository.Core;
using Parametriz.AutoNFP.Domain.Instituicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Repository
{
    public class InstituicaoRepository : Repository<Instituicao>, IInstituicaoRepository
    {
        public InstituicaoRepository(AutoNfpDbContext context) 
            : base(context)
        {
        }

        public override async Task<bool> EhUnico(Instituicao instituicao)
        {
            return !await _context.Instituicoes
                .AnyAsync(i => i.Cnpj.NumeroInscricao == instituicao.Cnpj.NumeroInscricao &&
                               i.Id != instituicao.Id);
        }
    }
}
