using Parametriz.AutoNFP.Api.ViewModels.Core;
using Parametriz.AutoNFP.Core.ValueObjects;

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

        public static Email ToDomain(this EmailViewModel emailViewModel)
        {
            return new Email(emailViewModel.Conta);
        }
    }
}
