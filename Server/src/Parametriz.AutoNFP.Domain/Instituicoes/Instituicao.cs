using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Instituicoes
{
    public class Instituicao : Entity
    {
        public string RazaoSocial { get; private set; }
        public CnpjCpf Cnpj { get; private set; }
        public Endereco Endereco { get; private set; }
        public bool Desativada { get; private set; }

        private readonly List<Usuario> _usuarios;
        public IReadOnlyCollection<Usuario> Usuarios => _usuarios.AsReadOnly();

        public Instituicao(Guid id, string razaoSocial, string cnpj)
            : base(id)
        {
            AlterarRazaoSocial(razaoSocial);
            Cnpj = new CnpjCpf(TipoPessoa.Juridica, cnpj);

            _usuarios = [];
        }

        protected Instituicao() { }

        public void AlterarRazaoSocial(string nome)
        {
            RazaoSocial = nome.Trim().ToUpper();
        }

        public void AlterarEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public void IncluirUsuario(Usuario usuario)
        {
            _usuarios.Add(usuario);
        }

        public void Desativar()
        {
            Desativada = true;
        }

        public void Ativar()
        {
            Desativada = false;
        }
    }
}
