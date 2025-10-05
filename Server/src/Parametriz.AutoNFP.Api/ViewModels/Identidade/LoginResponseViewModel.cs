namespace Parametriz.AutoNFP.Api.ViewModels.Identidade
{
    public class LoginResponseViewModel
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public TokenUsuarioViewModel UserToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
