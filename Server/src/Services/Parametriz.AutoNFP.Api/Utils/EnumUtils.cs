using Parametriz.AutoNFP.Api.ViewModels.Core;

namespace Parametriz.AutoNFP.Api.Utils
{
    public static class EnumUtils
    {
        public static IEnumerable<EnumViewModel> ToViewModel(Type type)
        {
            var enums = new List<EnumViewModel>();

            var keys = Enum.GetValues(type);

            foreach (var key in keys)
            {
                enums.Add(new EnumViewModel { Chave = (int)key, Valor = Enum.GetName(type, key).Replace("_", " ") });
            }

            return enums;
        }
    }
}
