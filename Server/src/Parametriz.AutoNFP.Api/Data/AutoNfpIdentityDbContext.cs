using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;
using Parametriz.AutoNFP.Api.Models;

namespace Parametriz.AutoNFP.Api.Data
{
    public class AutoNfpIdentityDbContext : IdentityDbContext, ISecurityKeyContext
    {
        public AutoNfpIdentityDbContext(DbContextOptions<AutoNfpIdentityDbContext> options)
            : base(options) 
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<KeyMaterial> SecurityKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AutoNfpIdentityDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
