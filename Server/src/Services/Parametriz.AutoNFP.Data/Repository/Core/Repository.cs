using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Core.DomainObjects;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Repository.Core
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected AutoNfpDbContext _context;

        protected Repository(AutoNfpDbContext context)
        {
            _context = context;
        }

        public abstract Task<bool> EhUnico(TEntity obj);

        public void Cadastrar(TEntity obj)
        {
            _context.Set<TEntity>().Add(obj);
        }

        public virtual async Task CadastrarAsync(TEntity obj)
        {
            await _context.Set<TEntity>().AddAsync(obj);
        }

        public void CadastrarLista(IEnumerable<TEntity> objs)
        {
            _context.Set<TEntity>().AddRange(objs);
        }

        public virtual async Task CadastrarListaAsync(IEnumerable<TEntity> objs)
        {
            await _context.Set<TEntity>().AddRangeAsync(objs);
        }

        public virtual void Atualizar(TEntity obj)
        {
            _context.Set<TEntity>().Update(obj);
        }

        public virtual void AtualizarLista(IEnumerable<TEntity> objs)
        {
            _context.Set<TEntity>().UpdateRange(objs);
        }

        public virtual void Excluir(TEntity obj)
        {
            _context.Set<TEntity>().Remove(obj);
        }

        public virtual void ExcluirLista(IEnumerable<TEntity> objs)
        {
            _context.Set<TEntity>().RemoveRange(objs);
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
