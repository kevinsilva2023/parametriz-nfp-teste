using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using NuGet.Packaging;
using Parametriz.AutoNFP.Api.Configs;
using Parametriz.AutoNFP.Api.Data;
using Parametriz.AutoNFP.Api.Models;
using Parametriz.AutoNFP.Api.ViewModels.Identidade;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Api.Application.JwtToken.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtService _jwksService;
        private readonly AppJwtConfig _appJwtConfig;
        private readonly AutoNfpIdentityDbContext _context;

        private IdentityUser _user;
        private ICollection<Claim> _userClaims;
        private ICollection<Claim> _jwtClaims;
        private ClaimsIdentity _identityClaims;

        public JwtTokenService(UserManager<IdentityUser> userManager,
                               IJwtService jwksService,
                               IOptions<AppJwtConfig> options,
                               AutoNfpIdentityDbContext contex)
        {
            _userManager = userManager;
            _jwksService = jwksService;
            _appJwtConfig = options.Value;

            _userClaims = [];
            _jwtClaims = [];
            _identityClaims = new ClaimsIdentity();
            _context = contex;
        }

        private static long ToUnixEpochDate(DateTime date) => 
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public async Task<LoginResponseViewModel> ObterLoginResponse(Guid instituicaoId, string email)
        {
            _user = await _userManager.FindByEmailAsync(email);
            await IncluirRolesUsuario();
            await IncluirClaimsUsuario();
            IncluirJwtClaims();

            return await GerarLoginResponse(instituicaoId);
        }

        private async Task IncluirRolesUsuario()
        {
            var userRoles = await _userManager.GetRolesAsync(_user);
            var userClaims = userRoles.Select(userRole => new Claim("role", userRole));

            _userClaims.AddRange(userClaims);
            _identityClaims.AddClaims(userClaims);
        }

        private async Task IncluirClaimsUsuario()
        {
            var userClaims = await _userManager.GetClaimsAsync(_user);

            _userClaims.AddRange(userClaims);
            _identityClaims.AddClaims(userClaims);
        }

        private void IncluirJwtClaims()
        {
            _jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, _user.Id.ToString()));
            _jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Email, _user.Email));
            _jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            _jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            _jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            _identityClaims.AddClaims(_jwtClaims);
        }

        private async Task<LoginResponseViewModel> GerarLoginResponse(Guid instituicaoId)
        {
            return new LoginResponseViewModel
            {
                AccessToken = GerarJwtToken(),
                ExpiresIn = TimeSpan.FromHours(_appJwtConfig.Expiration).TotalSeconds,
                UserToken = new TokenUsuarioViewModel
                {
                    Id = _user.Id,
                    Email = _user.Email,
                    InstituicaoId = instituicaoId,
                    Claims = _userClaims.Select(c => new TokenUsuarioClaimViewModel { Type = c.Type, Value = c.Value }).ToList()
                },
                RefreshToken = await GerarRefreshToken(instituicaoId)
            };
        }

        private string GerarJwtToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = _jwksService.GetCurrentSigningCredentials();

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appJwtConfig.Issuer,
                Audience = _appJwtConfig.Audience,
                Subject = _identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appJwtConfig.Expiration),
                SigningCredentials = key.Result
            });

            return tokenHandler.WriteToken(token);
        }

        private async Task<Guid> GerarRefreshToken(Guid instituicaoId)
        {
            var refreshToken = new RefreshToken
            {
                InstituicaoId = instituicaoId,
                UserName = _user.Email,
                ExpirationDate = DateTime.UtcNow.AddHours(_appJwtConfig.RefreshTokenExpiration)
            };

            _context.RefreshTokens.RemoveRange(_context.RefreshTokens.Where(u => u.UserName == _user.Email));
            await _context.RefreshTokens.AddAsync(refreshToken);

            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        public async Task<RefreshToken> ObterRefreshToken(Guid refreshToken)
        {
            var token = await _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Token == refreshToken);

            return token != null && token.ExpirationDate.ToLocalTime() > DateTime.Now ? token : null;
        }
    }
}
