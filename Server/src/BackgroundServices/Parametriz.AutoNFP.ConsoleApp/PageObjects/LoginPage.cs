using Parametriz.AutoNFP.ConsoleApp.SeleniumConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.PageObjects
{
    public class LoginPage : BasePage
    {
        public LoginPage(SeleniumHelper helper) 
            : base(helper)
        {
        }

        public void AcessarPagina()
        {
            Helper.IrParaUrl("https://www.nfp.fazenda.sp.gov.br/login.aspx");
        }

        public void ClicarAcessoViaCertificadoDigital()
        {
            Helper.ClicarBotaoPorId("imgBtnAcessoCertCPF");
        }
    }
}
