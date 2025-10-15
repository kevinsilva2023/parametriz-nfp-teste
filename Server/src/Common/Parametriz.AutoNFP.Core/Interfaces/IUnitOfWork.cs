using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task BeginTransaction();

        Task<bool> Commit();

        Task CommitTransaction();

        Task RollbackTransaction();

        void ClearChangeTracker();
    }
}
