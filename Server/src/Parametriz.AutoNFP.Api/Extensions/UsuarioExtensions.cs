using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Usuarios;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class UsuarioExtensions
    {
        public static UsuarioViewModel ToViewModel(this Usuario usuario)
        {
            return new UsuarioViewModel
            {
                Id = usuario.Id,
                InstituicaoId = usuario.InstituicaoId,
                Nome = usuario.Nome,
                Email = usuario.Email.ToViewModel(),
                Desativado = usuario.Desativado
            };
        }

        public static IEnumerable<UsuarioViewModel> ToViewModel(this IEnumerable<Usuario> usuarios)
        {
            return usuarios.Select(u => u.ToViewModel());
        }
    }
}
