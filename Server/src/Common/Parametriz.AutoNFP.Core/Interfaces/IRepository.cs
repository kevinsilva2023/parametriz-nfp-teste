using Parametriz.AutoNFP.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Core.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity 
    {
        Task<bool> EhUnico(TEntity obj);
                
        void Cadastrar(TEntity obj);
        Task CadastrarAsync(TEntity obj);
        void CadastrarLista(IEnumerable<TEntity> objs);
        Task CadastrarListaAsync(IEnumerable<TEntity> objs);

        void Atualizar(TEntity obj);

        void AtualizarLista(IEnumerable<TEntity> objs);

        void Excluir(TEntity obj);

        void ExcluirLista(IEnumerable<TEntity> objs);
    }
}
