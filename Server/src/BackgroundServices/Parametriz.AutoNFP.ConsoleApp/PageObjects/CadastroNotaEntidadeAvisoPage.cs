using Parametriz.AutoNFP.ConsoleApp.SeleniumConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.ConsoleApp.PageObjects
{
    public class CadastroNotaEntidadeAvisoPage : BasePage
    {
        public CadastroNotaEntidadeAvisoPage(SeleniumHelper helper) 
            : base(helper)
        {
        }

        public void AcessarPagina()
        {
            Helper.IrParaUrl("https://www.nfp.fazenda.sp.gov.br/EntidadesFilantropicas/CadastroNotaEntidadeAviso.aspx");
        }

        public void ClicarEmProsseguir()
        {
            Helper.ClicarBotaoPorId("ctl00_ConteudoPagina_btnOk");
        }

        public void SelecionarEntidade(string entidade)
        {
            Helper.SelecionarTextoDropDownPorId("ddlEntidadeFilantropica", entidade);
        }

        public void ClicarEmNovaNota()
        {
            Helper.ClicarBotaoPorId("ctl00_ConteudoPagina_btnNovaNota");
        }

        public void FecharModal()
        {
            Helper.ClicarPorXPath("//div[contains(@class,'ui-dialog-buttonset')]/button[span[text()='Sim']]");
        }
    }
}
