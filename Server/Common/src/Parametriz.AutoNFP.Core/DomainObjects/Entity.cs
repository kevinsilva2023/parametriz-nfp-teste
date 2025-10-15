using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        public Entity(Guid id)
        {
            Id = id;
        }

        protected Entity() { }
    }
}
