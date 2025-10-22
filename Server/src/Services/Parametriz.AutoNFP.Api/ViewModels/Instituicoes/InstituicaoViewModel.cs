using Parametriz.AutoNFP.Api.ViewModels.Core;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Instituicoes
{
    public class InstituicaoViewModel
    {
        public Guid Id { get; set; }
        public string RazaoSocial { get; set; }

        public string Cnpj { get; set; }

        [Display(Name = "Nome da Entidade na NFP.")]
        [Required(ErrorMessage = "Favor preencher o nome da entidade na nota fiscal paulista.")]
        [MaxLength(256, ErrorMessage = "Nome da entidade na nota fiscal paulista deve ser preenchido com no máximo {1} caracteres.")]
        public string EntidadeNomeNFP { get; set; }

        public EnderecoViewModel Endereco { get; set; }

        public bool Desativada { get; set; }
    }
}
