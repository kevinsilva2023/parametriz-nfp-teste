using Parametriz.AutoNFP.Api.ViewModels.Core;
using System.ComponentModel.DataAnnotations;

namespace Parametriz.AutoNFP.Api.ViewModels.Voluntarios
{
    public class VoluntarioViewModel
    {
        public Guid Id { get; set; }
        public Guid InstituicaoId { get; set; }
        public string Nome { get; set; }
        public CnpjCpfViewModel CnpjCpf { get; set; }
        public string Requerente { get; set; }
        public DateTime ValidoAPartirDe { get; set; }
        public DateTime ValidoAte { get; set; }
        public string Emissor { get; set; }
        public string Status => ObterStatus();

        private string ObterStatus()
        {
            if (ValidoAPartirDe > DateTime.Now.Date)
                return "INVÁLIDO";

            if (ValidoAte.Date < DateTime.Now.Date)
                return "VENCIDO";

            var diasParaVencer = (ValidoAte.Date - DateTime.Now.Date).Days;

            if (diasParaVencer <= 30)
                return $"{diasParaVencer} DIAS PARA VENCER.";

            return "VÁLIDO";
        }
    }
}
