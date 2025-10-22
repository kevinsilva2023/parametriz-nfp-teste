using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Core.ValueObjects;

namespace Parametriz.AutoNFP.Api.Extensions.Core
{
    public static class EnderecoExtensions
    {
        public static Endereco ToDomain(this EnderecoViewModel enderecoViewModel)
        {
            return new Endereco(enderecoViewModel.TipoLogradouro, enderecoViewModel.Logradouro, enderecoViewModel.Numero,
                enderecoViewModel.Complemento, enderecoViewModel.Bairro, enderecoViewModel.CEP, enderecoViewModel.Municipio,
                enderecoViewModel.UF);
        }
    }
}
