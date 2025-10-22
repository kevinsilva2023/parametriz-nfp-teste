using Parametriz.AutoNFP.Api.ViewModels.Core;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Instituicoes
{
    public class InstituicaoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "Favor preencher a razão social.")]
        [MaxLength(256, ErrorMessage = "Razão social deve ser preenchida com no máximo {1} caracteres.")]
        public string RazaoSocial { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Favor preencher o CNPJ.")]
        [StringLength(14, ErrorMessage = "CNPJ deve ser preenchido com {1} digitos.", MinimumLength = 14)]
        public string Cnpj { get; set; }

        [Display(Name = "Nome da Entidade na NFP.")]
        [Required(ErrorMessage = "Favor preencher o nome da entidade na nota fiscal paulista.")]
        [MaxLength(256, ErrorMessage = "Nome da entidade na nota fiscal paulista deve ser preenchido com no máximo {1} caracteres.")]
        public string EntidadeNomeNFP { get; set; }

        public EnderecoViewModel Endereco { get; set; }

        public bool Desativada { get; set; }
    }
}
