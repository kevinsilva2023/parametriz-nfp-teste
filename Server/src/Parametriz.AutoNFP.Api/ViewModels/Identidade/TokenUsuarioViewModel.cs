namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class TokenUsuarioViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public Guid InstituicaoId { get; set; }
        public ICollection<TokenUsuarioClaimViewModel> Claims { get; set; }

        public TokenUsuarioViewModel()
        {
            Claims = [];
        }
    }
}
