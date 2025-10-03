using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Favor preencher o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Favor preencher a senha.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
