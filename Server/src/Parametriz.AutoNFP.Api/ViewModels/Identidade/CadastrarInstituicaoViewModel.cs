using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class CadastrarInstituicaoViewModel
    {
        [Display(Name ="Razão Social")]
        [Required(ErrorMessage = "Favor preencher a razão social.")]
        [MaxLength(256, ErrorMessage = "Razão social deve ser preenchida com no máximo {1} caracteres.")]
        public string RazaoSocial { get; set; }

        [Display(Name ="CNPJ")]
        [Required(ErrorMessage ="Favor preencher o CNPJ.")]
        [StringLength(14, ErrorMessage = "CNPJ deve ser preenchido com {1} digitos.", MinimumLength = 14)]
        public string Cnpj { get; set; }

        [Display(Name ="Nome Voluntário")]
        [Required(ErrorMessage = "Favor preencher o nome do voluntário.")]
        [MaxLength(256, ErrorMessage = "Nome deve ser preenchido com no máximo {1} caracteres.")]
        public string VoluntarioNome { get; set; }

        [Display(Name ="E-mail")]
        [Required(ErrorMessage ="Favor preencher o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(256, ErrorMessage = "E-mail deve ser preenchido com no máximo {1} caracteres.")]
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
