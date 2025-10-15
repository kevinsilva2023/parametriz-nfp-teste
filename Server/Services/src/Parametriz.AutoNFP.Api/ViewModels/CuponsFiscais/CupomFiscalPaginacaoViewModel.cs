namespace Parametriz.AutoNFP.Api.ViewModels.CuponsFiscais
{
    public class CupomFiscalPaginacaoViewModel
    {
        public IEnumerable<CupomFiscalViewModel> CuponsFiscais { get; set; }
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int Processando { get; set; }
        public int Sucesso { get; set; }
        public int Erro { get; set; }
        public int Total { get; set; }
    }
}
