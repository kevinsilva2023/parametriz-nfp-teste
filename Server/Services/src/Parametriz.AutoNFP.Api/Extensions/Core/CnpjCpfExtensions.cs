using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Core.ValueObjects;

namespace Parametriz.AutoNFP.Api.Extensions.Core
{
    public static class CnpjCpfExtensions
    {
        public static CnpjCpfViewModel ToViewModel(this CnpjCpf cnpjCpf)
        {
            return new CnpjCpfViewModel
            {
                TipoPessoa = cnpjCpf.TipoPessoa,
                NumeroInscricao = cnpjCpf.NumeroInscricao
            };
        }
    }
}
