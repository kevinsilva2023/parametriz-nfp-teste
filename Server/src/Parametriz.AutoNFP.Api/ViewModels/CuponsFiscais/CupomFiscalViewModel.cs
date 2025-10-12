using Parametriz.AutoNFP.Api.ViewModels.Usuarios;
using Parametriz.AutoNFP.Domain.Core.Enums;

namespace Parametriz.AutoNFP.Api.ViewModels.CuponsFiscais
{
    public class CupomFiscalViewModel
    {
        public Guid Id { get; set; }
        public Guid InstituicaoId { get; set; }
        public string Chave { get; set; }
        public string Cnpj { get; set; }
        public DateTime MesEmissao { get; set; }
        public Guid CadastradoPorId { get; set; }
        public CupomFiscalStatus Status { get; set; }
        public string StatusNome => Enum.GetName(Status).Replace("_", " ");
        public DateTime? EnviadoEm { get; set; }
        public string MensagemErro { get; set; }

        public UsuarioViewModel CadastradoPor { get; set; }
    }
}
