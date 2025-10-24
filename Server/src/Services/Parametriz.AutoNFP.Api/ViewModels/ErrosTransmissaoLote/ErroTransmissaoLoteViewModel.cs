using Parametriz.AutoNFP.Api.ViewModels.Voluntarios;

namespace Parametriz.AutoNFP.Api.ViewModels.ErrosTransmissaoLote
{
    public class ErroTransmissaoLoteViewModel
    {
        public Guid Id { get; set; }
        public Guid InstituicaoId { get; set; }
        public Guid? VoluntarioId { get; set; }
        public DateTime Data { get; set; }
        public string Mensagem { get; set; }

        public VoluntarioViewModel Voluntario { get; set; }
    }
}
