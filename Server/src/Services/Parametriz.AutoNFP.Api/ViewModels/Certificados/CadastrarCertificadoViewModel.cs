using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Certificados
{
    public class CadastrarCertificadoViewModel
    {
        [Required(ErrorMessage = "Favor preencher o upload.")]
        public string Upload { get; set; }

        [Required(ErrorMessage ="Favor preencher a senha.")]
        public string Senha { get; set; }
    }
}
