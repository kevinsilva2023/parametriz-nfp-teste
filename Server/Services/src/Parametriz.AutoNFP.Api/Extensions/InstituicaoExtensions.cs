using Parametriz.AutoNFP.Api.ViewModels.Instituicoes;
using Parametriz.AutoNFP.Domain.Instituicoes;

namespace Parametriz.AutoNFP.Api.Extensions
{
    public static class InstituicaoExtensions
    {
        public static InstituicaoViewModel ToViewModel(this Instituicao instituicao)
        {
            return new InstituicaoViewModel
            {
                Id = instituicao.Id,
                RazaoSocial = instituicao.RazaoSocial
            };
        }
    }
}
