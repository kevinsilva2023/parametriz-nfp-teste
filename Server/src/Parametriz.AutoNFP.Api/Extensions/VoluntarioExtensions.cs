using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class VoluntarioExtensions
    {
        public static VoluntariosViewModel ToViewModel(this Voluntario voluntario)
        {
            return new VoluntariosViewModel
            {
                Id = voluntario.Id,
                Nome = voluntario.Nome,
                Email = voluntario.Email.ToViewModel(),
                Desativado = voluntario.Desativado
            };
        }
    }
}
