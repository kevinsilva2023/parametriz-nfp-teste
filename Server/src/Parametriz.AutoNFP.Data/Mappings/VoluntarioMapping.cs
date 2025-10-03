using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parametriz.AutoNFP.Domain.Voluntarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Mappings
{
    public class VoluntarioMapping : IEntityTypeConfiguration<Voluntario>
    {
        public void Configure(EntityTypeBuilder<Voluntario> builder)
        {
            builder.HasKey(pk => pk.Id);

            builder.HasOne(p => p.Instituicao)
                .WithMany(p => p.Voluntarios)
                .HasForeignKey(p => p.InstituicaoId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Property(p => p.Nome)
                .HasMaxLength(256);

            builder.OwnsOne(p => p.Email, e =>
            {
                e.Property(p => p.Conta)
                    .HasMaxLength(256)
                    .IsRequired()
                    .HasColumnName("Email");

                e.HasIndex(i => i.Conta)
                    .IsUnique();
            });

            builder.Property(p => p.Desativado)
                .IsRequired();

            builder.HasIndex(i => new { i.InstituicaoId, i.Nome })
                .IsUnique();

            builder.ToTable("Voluntarios");
        }
    }
}
