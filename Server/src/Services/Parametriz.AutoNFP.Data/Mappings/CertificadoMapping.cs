using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parametriz.AutoNFP.Domain.Certificados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Mappings
{
    public class CertificadoMapping : IEntityTypeConfiguration<Certificado>
    {
        public void Configure(EntityTypeBuilder<Certificado> builder)
        {
            builder.HasKey(pk => pk.Id);

            builder.HasOne(p => p.Voluntario)
                .WithOne(p => p.Certificado)
                .HasForeignKey<Certificado>(fk => fk.VoluntarioId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(p => p.Requerente)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(p => p.ValidoAPartirDe)
                .IsRequired();

            builder.Property(p => p.ValidoAte)
                .IsRequired();

            builder.Property(p => p.Emissor)
                .HasMaxLength(256)
                .IsRequired();
            
            builder.Property(p => p.Upload)
                .IsRequired();

            builder.Property(p => p.Senha)
                .IsRequired();

            builder.HasIndex(i => i.VoluntarioId)
                .IsUnique();

            builder.ToTable("Certificados");
        }
    }
}
