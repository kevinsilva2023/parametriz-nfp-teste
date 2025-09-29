using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Usuarios
{
    public class Usuario : Entity
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }

        public Usuario(Guid id, string nome, Email email)
            : base(id)
        {
            Email = email;
            AlterarNome(nome);
        }

        protected Usuario() { }

        public void AlterarNome(string nome)
        {
            Nome = nome.Trim().ToUpper();
        }
    }
}
