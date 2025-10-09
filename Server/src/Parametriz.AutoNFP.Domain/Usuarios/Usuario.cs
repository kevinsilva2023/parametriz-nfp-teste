using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Instituicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Usuarios
{
    public class Usuario : InstituicaoEntity
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public bool Desativado { get; private set; }

        public Instituicao Instituicao { get; private set; }

        public Usuario(Guid id, Guid instituicaoId, string nome, Email email)
            : base(id, instituicaoId)
        {
            Email = email;
            AlterarNome(nome);
        }

        protected Usuario() { }

        public void AlterarNome(string nome)
        {
            Nome = nome.Trim().ToUpper();
        }

        public void Desativar() 
        { 
            Desativado = true;  
        }

        public void Ativar()
        {
            Desativado = false;
        }
    }
}
