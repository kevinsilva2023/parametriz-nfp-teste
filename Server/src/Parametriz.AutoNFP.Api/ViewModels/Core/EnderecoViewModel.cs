using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Core
{
    public class EnderecoViewModel
    {
        [Display(Name = "Tipo Logradouro")]
        [StringLength(15, ErrorMessage = "Tipo de logradouro deve ser preenchido com no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 2)]
        public string TipoLogradouro { get; set; }

        [Display(Name = "Logradouro")]
        [StringLength(100, ErrorMessage = "Logradouro deve ser preenchido com no mínimo {2} e no máximo  {1} caracteres.", MinimumLength = 2)]
        public string Logradouro { get; set; }

        [Display(Name = "Número")]
        [MaxLength(10, ErrorMessage = "Número deve ser preenchido com no máximo {1} caracteres.")]
        public string Numero { get; set; }

        [Display(Name = "Complemento")]
        [MaxLength(50, ErrorMessage = "Complemento deve ser preenchido com no máximo {1} caracteres.")]
        public string Complemento { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(100, ErrorMessage = "Bairro deve ser preenchido com no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 2)]
        public string Bairro { get; set; }

        [Display(Name = "CEP")]
        [StringLength(8, ErrorMessage = "CEP deve ser preenchido com {1} dígitos numéricos.", MinimumLength = 8)]
        public string CEP { get; set; }

        [Display(Name = "Município")]
        [StringLength(50, ErrorMessage = "Município deve ser preenchido com no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 2)]
        public string Municipio { get; set; }

        [Display(Name = "UF")]
        [StringLength(2, ErrorMessage = "UF deve ser preenchida com {1} caracteres.", MinimumLength = 2)]
        public string UF { get; set; }
    }
}
