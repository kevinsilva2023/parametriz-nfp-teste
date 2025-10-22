using Parametriz.AutoNFP.Api.ViewModels.Core;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class CadastrarInstituicaoViewModel
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


        [Display(Name = "Nome Voluntário")]
        [Required(ErrorMessage = "Favor preencher o nome do voluntário.")]
        [MaxLength(256, ErrorMessage = "Nome do voluntário deve ser preenchido com no máximo {1} caracteres.")]
        public string VoluntarioNome { get; set; }

        [Required(ErrorMessage = "Favor preencher o CPF do voluntário.")]
        [StringLength(11, ErrorMessage = "CPF inválido.", MinimumLength = 11)]
        public string Cpf { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Favor preencher o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [MaxLength(256, ErrorMessage = "E-mail deve ser preenchido com no máximo {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Favor preencher um número para contato do voluntário.")]
        [StringLength(11, ErrorMessage = "Número para contato inválido.")]
        public string Contato { get; set; }

        public CadastrarInstituicaoViewModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
