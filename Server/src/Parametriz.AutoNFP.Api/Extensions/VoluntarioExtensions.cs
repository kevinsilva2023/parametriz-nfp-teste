using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class VoluntarioExtensions
    {
        public static UsuarioViewModel ToViewModel(this Usuario voluntario)
        {
            return new UsuarioViewModel
            {
                Id = voluntario.Id,
                Nome = voluntario.Nome,
                Email = voluntario.Email.ToViewModel(),
                Desativado = voluntario.Desativado
            };
        }
    }
}
