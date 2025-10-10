using Parametriz.AutoNFP.Domain.Core.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Core
{
    public class EmailViewModel
    {
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Favor preencher o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Conta { get; set; }
    }
}
