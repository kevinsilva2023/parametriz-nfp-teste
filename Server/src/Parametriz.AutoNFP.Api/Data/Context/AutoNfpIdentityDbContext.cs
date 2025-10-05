using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace Parametriz.AutoNFP.Api.Data.Context
{
    public class AutoNfpIdentityDbContext : IdentityDbContext, ISecurityKeyContext
    {
        public AutoNfpIdentityDbContext(DbContextOptions<AutoNfpIdentityDbContext> options)
            : base(options) 
        {
        }

        public DbSet<KeyMaterial> SecurityKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AutoNfpIdentityDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
