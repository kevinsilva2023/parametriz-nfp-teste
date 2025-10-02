using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Instituicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Voluntarios
{
    public class Voluntario : InstituicaoEntity
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }

        public Instituicao Instituicao { get; private set; }

        public Voluntario(Guid id, Guid instituicaoId, string nome, Email email)
            : base(id, instituicaoId)
        {
            Email = email;
            AlterarNome(nome);
        }

        protected Voluntario() { }

        public void AlterarNome(string nome)
        {
            Nome = nome.Trim().ToUpper();
        }
    }
}
