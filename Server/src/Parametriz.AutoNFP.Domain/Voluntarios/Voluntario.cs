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
        public string Emissor { get; private set; }
        public DateTime ValidoAPartirDe { get; private set; }
        public DateTime ValidoAte { get; private set; }
        public string Requerente { get; private set; }
        public string Upload { get; private set; }
        public string Senha { get; private set; }

        public Voluntario(Guid id, Guid instituicaoId, string emissor, DateTime validoAPartirDe, DateTime validoAte,
            string requerente, string upload, string senha)
                : base(id, instituicaoId)
        {
            Emissor = emissor;
            ValidoAPartirDe = validoAPartirDe;
            ValidoAte = validoAte;
            Requerente = requerente;
            Upload = upload;
            Senha = senha;
        }
    }
}
