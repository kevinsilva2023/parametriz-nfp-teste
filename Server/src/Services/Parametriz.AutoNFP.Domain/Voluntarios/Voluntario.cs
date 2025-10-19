using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Voluntarios
{
    public class Voluntario : InstituicaoEntity
    {
        public string EntidadeNomeNFP { get; private set; }
        public string Nome { get; private set; }
        public CnpjCpf CnpjCpf { get; private set; }
        public string Requerente { get; private set; }
        public DateTime ValidoAPartirDe { get; private set; }
        public DateTime ValidoAte { get; private set; }
        public string Emissor { get; private set; }
        public byte[] Upload { get; private set; }
        public byte[] Senha { get; private set; }

        public Voluntario(Guid id, Guid instituicaoId, string entidadeNomeNFP, string nome, CnpjCpf cnpjCpf, string requerente, 
            DateTime validoAPartirDe, DateTime validoAte, string emissor, byte[] upload, byte[] senha)
                : base(id, instituicaoId)
        {
            EntidadeNomeNFP = entidadeNomeNFP;
            Nome = nome;
            CnpjCpf = cnpjCpf;
            Requerente = requerente;
            ValidoAPartirDe = validoAPartirDe;
            ValidoAte = validoAte;
            Emissor = emissor;
            Upload = upload;
            Senha = senha;
        }

        protected Voluntario() { }
    }
}
