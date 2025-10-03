using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class DefinirSenhaViewModel
    {
        [Required(ErrorMessage = "Favor preencher o e-mail.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Favor preencher a senha.")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string SenhaConfirmacao { get; set; }

        public string Code { get; set; }
    }
}
