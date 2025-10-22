using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Domain.Certificados;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Certificados
{
    public class CertificadoViewModel
    {
        public Guid Id { get; set; }
        public Guid VoluntarioId { get; set; }
        public string Requerente { get; set; }
        public DateTime ValidoAPartirDe { get; set; }
        public DateTime ValidoAte { get; set; }
        public string Emissor { get; set; }
        public CertificadoStatus Status { get; set; }
        public string StatusNome => Enum.GetName(Status);
    }
}
