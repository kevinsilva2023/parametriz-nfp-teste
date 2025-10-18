using Parametriz.AutoNFP.ConsoleApp.SeleniumConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.PageObjects
{
    public abstract class BasePage
    {
        protected readonly SeleniumHelper Helper;

        protected BasePage(SeleniumHelper helper)
        {
            Helper = helper;
        }

    }
}
