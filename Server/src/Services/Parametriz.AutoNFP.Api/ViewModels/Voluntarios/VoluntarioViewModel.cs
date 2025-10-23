using Parametriz.AutoNFP.Api.ViewModels.Certificados;
using Parametriz.AutoNFP.Api.ViewModels.Core;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Voluntarios
{
    public class VoluntarioViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Instituição")]
        [Required(ErrorMessage = "Favor selecionar a instituição.")]
        public Guid InstituicaoId { get; set; }

        [Required(ErrorMessage = "Favor preencher o nome do usuário.")]
        [MaxLength(256, ErrorMessage = "Nome deve ser preenchido com no máximo {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Favor preencher o CPF.")]
        [StringLength(11, ErrorMessage = "CPF deve ser preenchido com {1} digitos.", MinimumLength = 11)]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Favor preencher o e-mail.")]
        [MaxLength(256, ErrorMessage = "E-mail deve ser preenchido com no máximo {1} caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Favor preencher o número para contato.")]
        [StringLength(11, ErrorMessage = "Número para contato deve ser preenchido com {1} digitos.")]
        public string Contato { get; set; }

        [Display(Name = "Foto")]
        public string FotoUpload { get; set; }
        public bool EmailConfirmado { get; set; }

        public bool Administrador { get; set; }

        public bool Desativado { get; set; }

        public CertificadoViewModel Certificado { get; set; }
    }
}
