using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Core.ValueObjects;

namespace Parametriz.AutoNFP.Api.Extensions.Core
{
    public static class EnderecoExtensions
    {
        public static Endereco ToDomain(this EnderecoViewModel enderecoViewModel)
        {
            return new Endereco(enderecoViewModel.Logradouro, enderecoViewModel.Numero,
                enderecoViewModel.Complemento, enderecoViewModel.Bairro, enderecoViewModel.CEP, enderecoViewModel.Municipio,
                enderecoViewModel.UF);
        }

        public static EnderecoViewModel ToViewModel(this Endereco endereco)
        {
            return new EnderecoViewModel
            {
                Logradouro = endereco.Logradouro,
                Numero = endereco.Numero,
                Complemento = endereco.Complemento,
                Bairro = endereco.Bairro,
                CEP = endereco.CEP,
                Municipio = endereco.Municipio,
                UF = endereco.UF
            };
        }
    }
}
