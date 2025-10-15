using Microsoft.EntityFrameworkCore.Storage;
using Parametriz.AutoNFP.Core.Interfaces;
using Parametriz.AutoNFP.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Uow
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AutoNfpDbContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(AutoNfpDbContext context)
        {
            _context = context;
            _transaction = null;
        }

        public async Task BeginTransaction()
        {
            if (_transaction is null)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task<bool> Commit()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task CommitTransaction()
        {
            await _transaction?.CommitAsync();
            _transaction = null;
        }

        public async Task RollbackTransaction()
        {
            await _transaction?.RollbackAsync();
            _transaction = null;
        }

        public void ClearChangeTracker()
        {
            _context.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
