using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.SeleniumConfig
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver(int port, bool headless)
        {
            IWebDriver webDriver = null;

            var options = new ChromeOptions();

            if (headless)
                options.AddArgument("--headless");

            var uri = new Uri($"http://localhost:{port}/");

            webDriver = new RemoteWebDriver(uri, options);

            return webDriver;
        }
    }
}
