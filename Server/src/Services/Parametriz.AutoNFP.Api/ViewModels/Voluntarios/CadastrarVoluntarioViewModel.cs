using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Voluntarios
{
    public class CadastrarVoluntarioViewModel
    {
        [Display(Name ="Nome da Entidade na NFP.")]
        [Required(ErrorMessage = "Favor preencher o nome da entidade na nota fiscal paulista.")]
        [MaxLength(256, ErrorMessage = "Nome da entidade na nota fiscal paulista deve ser preenchido com no máximo {1} caracteres.")]

        public string EntidadeNomeNFP { get; set; }
        [Required(ErrorMessage = "Favor preencher o upload.")]
        public string Upload { get; set; }

        [Required(ErrorMessage ="Favor preencher a senha.")]
        public string Senha { get; set; }
    }
}
