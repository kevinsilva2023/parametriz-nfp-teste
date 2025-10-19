using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class VoluntarioExtensions
    {
        public static VoluntarioViewModel ToViewModel(this Voluntario voluntario)
        {
            if (voluntario == null)
                return null;

            return new VoluntarioViewModel
            {
                Id = voluntario.Id,
                InstituicaoId = voluntario.InstituicaoId,
                EntidadeNomeNFP = voluntario.EntidadeNomeNFP,
                Nome = voluntario.Nome,
                CnpjCpf = voluntario.CnpjCpf.ToViewModel(),
                Requerente = voluntario.Requerente,
                ValidoAPartirDe = voluntario.ValidoAPartirDe,
                ValidoAte = voluntario.ValidoAte,
                Emissor = voluntario.Emissor
            };
        }
    }
}
