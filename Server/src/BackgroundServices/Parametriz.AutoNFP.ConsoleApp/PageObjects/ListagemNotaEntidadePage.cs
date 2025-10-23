using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
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
            var xPath = "/html/body/form/div[4]/div[6]/div[2]/div[2]/fieldset/div[4]/fieldset/input";

            Helper.ClickElementoPorXPath(xPath);
           
            var actions = new Actions(Helper.WebDriver);

            actions
                .KeyDown(Keys.Control)
                .SendKeys("a")
                .KeyUp(Keys.Control)
                .SendKeys(Keys.Delete);

            Helper.PreencherTextBoxPorXPath(xPath, chave);
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
