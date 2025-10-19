using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransactionAsync();

        bool Commit();
        Task<bool> CommitAsync();

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();

        void ClearChangeTracker();
        
    }
}
