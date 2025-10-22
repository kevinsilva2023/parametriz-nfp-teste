using Parametriz.AutoNFP.Core.DomainObjects;
using Parametriz.AutoNFP.Core.ValueObjects;
using Parametriz.AutoNFP.Domain.Core.DomainObjects;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Domain.Certificados
{
    public class Certificado : Entity
    {
        public Guid VoluntarioId { get; set; }
        public string Requerente { get; private set; }
        public DateTime ValidoAPartirDe { get; private set; }
        public DateTime ValidoAte { get; private set; }
        public string Emissor { get; private set; }
        public byte[] Upload { get; private set; }
        public byte[] Senha { get; private set; }

        public CertificadoStatus Status => ObterStatus();

        public Voluntario Voluntario { get; private set; }

        public Certificado(Guid id, Guid voluntarioId, string requerente, 
            DateTime validoAPartirDe, DateTime validoAte, string emissor, byte[] upload, byte[] senha)
                : base(id)
        {
            VoluntarioId = voluntarioId;
            Requerente = requerente;
            ValidoAPartirDe = validoAPartirDe;
            ValidoAte = validoAte;
            Emissor = emissor;
            Upload = upload;
            Senha = senha;
        }

        protected Certificado() { }

        private CertificadoStatus ObterStatus()
        {
            if (ValidoAte.Date < DateTime.Now.Date)
                return CertificadoStatus.VENCIDO;

            var diasParaVencer = (ValidoAte.Date - DateTime.Now.Date).Days;

            if (diasParaVencer <= 30)
                return CertificadoStatus.RENOVAR;

            return CertificadoStatus.VÁLIDO;
        }
    }
}
