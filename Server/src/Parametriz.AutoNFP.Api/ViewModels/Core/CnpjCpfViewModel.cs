using Parametriz.AutoNFP.Domain.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Core
{
    public class CnpjCpfViewModel
    {
        [Display(Name = "Tipo Pessoa")]
        public TipoPessoa TipoPessoa { get; set; }

        [Display(Name = @"CNPJ/CPF")]
        public string NumeroInscricao { get; set; }
    }
}
