using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Domain.Core.ValueObjects;

namespace Parametriz.AutoNFP.Api.Extensions.Core
{
    public static class EmailExtensions
    {
        public static EmailViewModel ToViewModel(this Email email)
        {
            return new EmailViewModel
            {
                Conta = email.Conta
            };
        }
    }
}
