using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.SeleniumConfig
{
    public class SeleniumHelper : IDisposable
    {
        public IWebDriver WebDriver;
        public WebDriverWait Wait;

        public SeleniumHelper(int port, bool headless = true)
        {
            WebDriver = WebDriverFactory.CreateWebDriver(port, headless);
            WebDriver.Manage().Window.Maximize();
            Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(5));
        }

        public string ObterUrl()
        {
            return WebDriver.Url;
        }

        public void IrParaUrl(string url)
        {
            WebDriver.Navigate().GoToUrl(url);
        }

        public bool ValidarConteudoUrl(string conteudo)
        {
            return Wait.Until(driver => driver.Url.Contains(conteudo));
        }

        public void ClicarLinkPorTexto(string linkText)
        {
            var link = Wait.Until(driver => driver.FindElement(By.LinkText(linkText)));
            link.Click();
        }

        public void ClicarBotaoPorId(string botaoId)
        {
            var botao = Wait.Until(driver => driver.FindElement(By.Id(botaoId)));
            botao.Click();
        }

        public void ClicarPorXPath(string xPath)
        {
            var elemento = Wait.Until(driver => driver.FindElement(By.XPath(xPath)));
            elemento.Click();
        }

        public IWebElement ObterElementoPorClasse(string classeCss)
        {
            return Wait.Until(driver => driver.FindElement(By.ClassName(classeCss)));
        }

        public IWebElement ObterElementoPorXPath(string xPath)
        {
            return Wait.Until(driver => driver.FindElement(By.XPath(xPath)));
        }

        public void PreencherTextBoxPorId(string idCampo, string valorCampo)
        {
            var campo = Wait.Until(driver => driver.FindElement(By.Id(idCampo)));
            campo.SendKeys(valorCampo);
        }

        public void PreencherTextBoxPorXPath(string xPath, string valorCampo)
        {
            var campo = Wait.Until(driver => driver.FindElement(By.XPath(xPath)));
            campo.SendKeys(valorCampo);
        }

        public void SelecionarValorDropDownPorId(string idCampo, string valorCampo)
        {
            var campo = Wait.Until(driver => driver.FindElement(By.Id(idCampo)));
            var selectElement = new SelectElement(campo);
            selectElement.SelectByValue(valorCampo);
        }

        public void SelecionarTextoDropDownPorId(string idCampo, string valorCampo)
        {
            var campo = Wait.Until(driver => driver.FindElement(By.Id(idCampo)));
            var selectElement = new SelectElement(campo);
            selectElement.SelectByText(valorCampo);
        }

        public string ObterTextoElementoPorClasseCss(string className)
        {
            return Wait.Until(driver => driver.FindElement(By.ClassName(className))).Text;
        }

        public string ObterTextoElementoPorId(string id)
        {
            return Wait.Until(driver => driver.FindElement(By.Id(id))).Text;
        }

        public string ObterTextoElementoPorXPath(string xpath)
        {
            return Wait.Until(driver => driver.FindElement(By.XPath(xpath))).Text;
        }

        public string ObterValorTextBoxPorId(string id)
        {
            return Wait.Until(driver => driver.FindElement(By.Id(id)))
                .GetAttribute("value");
        }

        public IEnumerable<IWebElement> ObterListaPorClasse(string className)
        {
            return Wait.Until(driver => driver.FindElements(By.ClassName(className)));
        }

        public bool ValidarSeElementoExistePorId(string id)
        {
            try
            {
                var elemento = WebDriver.FindElement(By.Id(id));

                return elemento != null;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public void VoltarNavegacao(int vezes = 1)
        {
            for (var i = 0; i < vezes; i++)
            {
                WebDriver.Navigate().Back();
            }
        }

        //public void ObterScreenShot(string nome)
        //{
        //    SalvarScreenShot(WebDriver.TakeScreenshot(), string.Format("{0}_" + nome + ".png", DateTime.Now.ToFileTime()));
        //}

        //private void SalvarScreenShot(Screenshot screenshot, string fileName)
        //{
        //    screenshot.SaveAsFile($"{Configuration.FolderPicture}{fileName}", ScreenshotImageFormat.Png);
        //}

        public void Dispose()
        {
            WebDriver.Quit();
            WebDriver.Dispose();
        }
    }
}
