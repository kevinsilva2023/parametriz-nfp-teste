using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Core.Enums;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Usuarios;
using Parametriz.AutoNFP.Domain.Voluntarios;
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

        public Voluntario Voluntario { get; private set; }

        private readonly List<Usuario> _usuarios;
        public IReadOnlyCollection<Usuario> Usuarios => _usuarios.AsReadOnly();

        private readonly List<CupomFiscal> _cuponsFiscais;
        public IReadOnlyCollection<CupomFiscal> CuponsFiscais => _cuponsFiscais.AsReadOnly();

        public Instituicao(Guid id, string razaoSocial, string cnpj)
            : base(id)
        {
            AlterarRazaoSocial(razaoSocial);
            Cnpj = new CnpjCpf(TipoPessoa.Juridica, cnpj);

            _usuarios = [];
            _cuponsFiscais = [];
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
