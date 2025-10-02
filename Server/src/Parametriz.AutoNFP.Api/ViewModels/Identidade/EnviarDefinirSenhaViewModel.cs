using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class EnviarDefinirSenhaViewModel
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Favor preencher o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(256, ErrorMessage = "E-mail deve ser preenchido com no máximo {1} caracteres.")]
        public string Email { get; set; }
    }
}
