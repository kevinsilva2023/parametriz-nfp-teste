using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.CuponsFiscais
{
    public class CadastrarCupomFiscalViewModel
    {
        [Required(ErrorMessage = "Favor preencher o QRCode.")]
        public string QrCode { get; set; }
    }
}
