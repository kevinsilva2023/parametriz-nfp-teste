namespace Parametriz.AutoNFP.Api.Configs
{
    public class AppJwtConfig
    {
        public int Expiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
