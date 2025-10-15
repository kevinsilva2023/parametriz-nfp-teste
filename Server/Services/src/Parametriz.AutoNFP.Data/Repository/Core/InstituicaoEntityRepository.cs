using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Data.Context;
using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Repository.Core
{
    public abstract class InstituicaoEntityRepository<TEntity> : Repository<TEntity>, 
        IInstituicaoEntityRepository<TEntity> where TEntity : InstituicaoEntity
    {
        protected InstituicaoEntityRepository(AutoNfpDbContext context) 
            : base(context)
        {
        }

        public virtual async Task<bool> ExisteId(Guid id, Guid instituicaoId)
        {
            return await _context.Set<TEntity>()
                .AnyAsync(p => p.InstituicaoId == instituicaoId &&
                               p.Id == id);
        }

        public virtual async Task<TEntity> ObterPorId(Guid id, Guid instituicaoId)
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.InstituicaoId == instituicaoId &&
                                           p.Id == id);            
        }
    }
}
