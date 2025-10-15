using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class ConfirmarEmailViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

        public bool DefinirSenha { get; set; }
    }
}
