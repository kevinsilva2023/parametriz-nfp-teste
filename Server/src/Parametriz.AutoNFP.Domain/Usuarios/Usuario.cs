using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
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
        public bool Administrador { get; private set; }
        public bool Desativado { get; private set; }

        private readonly List<CupomFiscal> _cuponsFiscais;
        public IReadOnlyCollection<CupomFiscal> CuponsFiscais => _cuponsFiscais.AsReadOnly();

        public Usuario(Guid id, Guid instituicaoId, string nome, Email email, bool administrador)
            : base(id, instituicaoId)
        {
            Email = email;
            AlterarNome(nome);
            AlterarAdministrador(administrador);
            
            _cuponsFiscais = [];
        }

        protected Usuario() { }

        public void AlterarNome(string nome)
        {
            Nome = nome.Trim().ToUpper();
        }

        public void AlterarAdministrador(bool administrador)
        {
            Administrador = administrador;
        }

        public void Desativar() 
        {
            Administrador = false;
            Desativado = true;  
        }

        public void Ativar()
        {
            Desativado = false;
        }
    }
}
