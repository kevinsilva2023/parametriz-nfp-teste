namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class TokenUsuarioViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public TokenInstituicaoViewModel Instituicao { get; set; }
        public ICollection<TokenUsuarioClaimViewModel> Claims { get; set; }

        public TokenUsuarioViewModel()
        {
            Claims = [];
        }
    }
}
