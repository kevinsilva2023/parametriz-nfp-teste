using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class CadastrarVoluntarioViewModel
    {
        [Display(Name ="Instituição")]
        [Required(ErrorMessage = "Favor preencher a instituição.")]
        public Guid InstituicaoId { get; set; }

        [Required(ErrorMessage = "Favor preencher o nome.")]
        [MaxLength(256, ErrorMessage = "Nome deve ser preenchido com no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Favor preencher o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(256, ErrorMessage = "E-mail deve ser preenchido com no máximo {1} caracteres.")]
        public string Email { get; set; }
    }
}
