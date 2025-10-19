using Parametriz.AutoNFP.ConsoleApp.SeleniumConfig;
using Parametriz.AutoNFP.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.PageObjects
{
    public class ListagemNotaEntidadePage : BasePage
    {
        public ListagemNotaEntidadePage(SeleniumHelper helper) 
            : base(helper)
        {
        }

        public void PreencherChaveDeAcesso(string chave)
        {
            Helper.PreencherTextBoxPorXPath("/html/body/form/div[4]/div[6]/div[2]/div[2]/fieldset/div[4]/fieldset/input", chave);
        }

        public void ClicarEmSalvarNota()
        {
            Helper.ClicarBotaoPorId("btnSalvarNota");
        }

        public KeyValuePair<bool, string> CapturarRetorno()
        {
            if (Helper.ValidarSeElementoExistePorId("lblInfo"))
                return new KeyValuePair<bool, string>(true, Helper.ObterTextoElementoPorId("lblInfo"));

            if (Helper.ValidarSeElementoExistePorId("lblErro"))
                return new KeyValuePair<bool, string>(false, Helper.ObterTextoElementoPorId("lblErro"));

            return new KeyValuePair<bool, string>(false, "Não foi possível capturar retorno.");
        }
    }
}
