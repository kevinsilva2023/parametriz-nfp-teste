using Parametriz.AutoNFP.Domain.Instituicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Core.DomainObjects
{
    public abstract class InstituicaoEntity : Entity
    {
        public Guid InstituicaoId { get; private set; }

        public Instituicao Instituicao { get; private set; }

        public InstituicaoEntity(Guid id, Guid instituicaoId)
            : base(id)
        {
            InstituicaoId = instituicaoId;
        }

        protected InstituicaoEntity() { }
    }
}
