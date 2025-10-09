using Parametriz.AutoNFP.Api.ViewModels.Core;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Usuarios
{
    public class UsuarioViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name ="Instituição")]
        [Required(ErrorMessage = "Favor preencher a instituição.")]
        public Guid InstituicaoId { get; set; }

        [Required(ErrorMessage = "Favor preencher o nome do usuário.")]
        [MaxLength(256, ErrorMessage = "Nome deve ser preenchido com no máximo {1} caracteres.")]
        public string Nome { get; set; }

        public EmailViewModel Email { get; set; }

        public bool Desativado { get; set; }
    }
}
