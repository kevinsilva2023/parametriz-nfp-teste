using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.ViewModels.Certificados;
using Parametriz.AutoNFP.Domain.Certificados;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class VoluntarioExtensions
    {
        public static CertificadoViewModel ToViewModel(this Certificado certificado)
        {
            if (certificado == null)
                return null;

            return new CertificadoViewModel
            {
                Id = certificado.Id,
                VoluntarioId = certificado.VoluntarioId,
                Nome = certificado.Nome,
                Cpf = certificado.Cpf.NumeroInscricao,
                Requerente = certificado.Requerente,
                ValidoAPartirDe = certificado.ValidoAPartirDe,
                ValidoAte = certificado.ValidoAte,
                Emissor = certificado.Emissor
            };
        }
    }
}
