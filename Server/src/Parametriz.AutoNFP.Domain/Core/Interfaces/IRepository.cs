using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Core.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity 
    {
        Task<bool> EhUnico(TEntity obj);
                
        Task Cadastrar(TEntity obj);

        Task CadastrarLista(IEnumerable<TEntity> objs);

        void Atualizar(TEntity obj);

        void AtualizarLista(IEnumerable<TEntity> objs);

        void Excluir(TEntity obj);

        void ExcluirLista(IEnumerable<TEntity> objs);
    }
}
