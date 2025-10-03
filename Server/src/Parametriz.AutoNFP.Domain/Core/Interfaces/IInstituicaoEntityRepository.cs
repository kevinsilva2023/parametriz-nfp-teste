using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Core.Interfaces
{
    public interface IInstituicaoEntityRepository<TEntity> : IRepository<TEntity> where TEntity : InstituicaoEntity
    {
        Task<bool> ExisteId(Guid id, Guid instituicaoId);
        Task<TEntity> ObterPorId(Guid id, Guid instituicaoId);
    }
}
