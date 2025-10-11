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
                .WithOne(p => p.Voluntario)
                .HasForeignKey<Voluntario>(fk => fk.InstituicaoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(p => p.Nome)
                .HasMaxLength(256)
                .IsRequired();

            builder.OwnsOne(p => p.CnpjCpf, c =>
            {
                c.Property(p => p.TipoPessoa)
                     .HasColumnName("TipoPessoa")
                     .HasMaxLength(1)
                     .IsFixedLength()
                     .IsRequired();

                c.Property(p => p.NumeroInscricao)
                    .HasMaxLength(14)
                    .HasColumnName("CnpjCpf")
                    .IsRequired();
            });

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
                .HasColumnType("text")
                .IsRequired();

            builder.HasIndex(i => i.InstituicaoId)
                .IsUnique();

            builder.ToTable("Voluntarios");
        }
    }
}
