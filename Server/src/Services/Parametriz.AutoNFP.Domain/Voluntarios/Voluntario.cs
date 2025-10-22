using Parametriz.AutoNFP.Core.Enums;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
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
        public CnpjCpf Cpf { get; private set; }
        public Email Email { get; private set; }
        public string Contato { get; private set; }
        public string FotoUpload { get; private set; }
        public bool Administrador { get; private set; }
        public bool Desativado { get; private set; }

        public Certificado Certificado { get; private set; }

        private readonly List<CupomFiscal> _cuponsFiscais;
        public IReadOnlyCollection<CupomFiscal> CuponsFiscais => _cuponsFiscais.AsReadOnly();

        public Voluntario(Guid id, Guid instituicaoId, string nome, string cpf, string email, string contato, bool administrador)
            : base(id, instituicaoId)
        {
            AlterarNome(nome);
            Cpf = new CnpjCpf(TipoPessoa.Fisica, cpf);
            Email = new Email(email);
            AlterarContato(contato);
            AlterarAdministrador(administrador);
            
            _cuponsFiscais = [];
        }

        protected Voluntario() { }

        public void AlterarNome(string nome)
        {
            Nome = nome.Trim().ToUpper();
        }

        public void AlterarContato(string contato)
        {
            Contato = contato;
        }

        public void AlterarFotoUpload(string fotoUpload)
        {
            FotoUpload = fotoUpload;
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
