using Parametriz.AutoNFP.Api.Extensions.Core;
using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;
using Parametriz.AutoNFP.Domain.Voluntarios;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class VoluntarioExtensions
    {
        public static VoluntarioViewModel ToViewModel(this Voluntario voluntario)
        {
            return new VoluntarioViewModel
            {
                Id = voluntario.Id,
                InstituicaoId = voluntario.InstituicaoId,
                Nome = voluntario.Nome,
                Cpf = voluntario.Cpf.NumeroInscricao,
                Email = voluntario.Email.Conta,
                Contato = voluntario.Contato,
                FotoUpload = voluntario.FotoUpload,
                Administrador = voluntario.Administrador,
                Desativado = voluntario.Desativado
            };
        }

        public static IEnumerable<VoluntarioViewModel> ToViewModel(this IEnumerable<Voluntario> usuarios)
        {
            return usuarios.Select(u => u.ToViewModel());
        }
    }
}
