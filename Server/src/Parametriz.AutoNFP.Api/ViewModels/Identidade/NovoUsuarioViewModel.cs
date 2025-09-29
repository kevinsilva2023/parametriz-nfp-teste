using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class NovoUsuarioViewModel
    {
        [Display(Name ="E-mail")]
        [Required(ErrorMessage ="Favor preencher o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Favor preencher a senha.")]
        [StringLength(50, ErrorMessage = "A senha deve ser preenchida com no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name ="Confirmação de Senha")]
        [Compare("Senha", ErrorMessage = "Confirmação de senha não confere.")]
        [DataType(DataType.Password)]
        public string SenhaConfirmacao { get; set; }
    }
}
