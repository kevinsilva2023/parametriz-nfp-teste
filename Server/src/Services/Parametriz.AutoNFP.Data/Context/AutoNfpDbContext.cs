using Microsoft.EntityFrameworkCore;
using Parametriz.AutoNFP.Domain.Certificados;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using Parametriz.AutoNFP.Domain.Instituicoes;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Context
{
    public class AutoNfpDbContext : DbContext
    {
        public AutoNfpDbContext(DbContextOptions<AutoNfpDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public DbSet<CupomFiscal> CuponsFiscais { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Certificado> Certificados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoNfpDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
