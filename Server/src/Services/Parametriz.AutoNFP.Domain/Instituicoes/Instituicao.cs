using Parametriz.AutoNFP.Core.DomainObjects;
using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
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
        public string EntidadeNomeNFP { get; private set; }
        public Endereco Endereco { get; private set; }
        public bool Desativada { get; private set; }

        private readonly List<Voluntario> _voluntarios;
        public IReadOnlyCollection<Voluntario> Voluntarios => _voluntarios.AsReadOnly();

        private readonly List<CupomFiscal> _cuponsFiscais;
        public IReadOnlyCollection<CupomFiscal> CuponsFiscais => _cuponsFiscais.AsReadOnly();

        public Instituicao(Guid id, string razaoSocial, string cnpj, string entidadeNomeNFP, Endereco endereco)
            : base(id)
        {
            AlterarRazaoSocial(razaoSocial);
            Cnpj = new CnpjCpf(TipoPessoa.Juridica, cnpj);
            AlterarEntidadeNomeNFP(entidadeNomeNFP);
            AlterarEndereco(endereco);
            
            _voluntarios = [];
            _cuponsFiscais = [];
        }

        protected Instituicao() { }

        public void AlterarRazaoSocial(string nome)
        {
            RazaoSocial = nome.Trim().ToUpper();
        }

        public void AlterarEntidadeNomeNFP(string entidadeNomeNFP)
        {
            EntidadeNomeNFP = entidadeNomeNFP;
        }

        public void AlterarEndereco(Endereco endereco)
        {
            Endereco = endereco;
        }

        public void IncluirVoluntario(Voluntario voluntario)
        {
            _voluntarios.Add(voluntario);
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
