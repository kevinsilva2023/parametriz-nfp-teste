using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parametriz.AutoNFP.Domain.CuponsFiscais;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Mappings
{
    public class CupomFiscalMapping : IEntityTypeConfiguration<CupomFiscal>
    {
        public void Configure(EntityTypeBuilder<CupomFiscal> builder)
        {
            builder.HasKey(pk => pk.Id);

            builder.HasOne(p => p.Instituicao)
                .WithMany(p => p.CuponsFiscais)
                .HasForeignKey(fk => fk.InstituicaoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.OwnsOne(p => p.ChaveDeAcesso, c =>
            {
                c.Property(p => p.Chave)
                    .HasColumnName("Chave")
                    .HasMaxLength(44)
                    .IsFixedLength()
                    .IsRequired();

                c.Ignore(i => i.ExisteChave);

                c.Property(p => p.Competencia)
                    .HasColumnName("Competencia")
                    .IsRequired();

                c.Property(p => p.Numero)
                    .HasColumnName("Numero")
                    .IsRequired();

                c.OwnsOne(c => c.Cnpj, n =>
                {
                    n.Ignore(i => i.TipoPessoa);

                    n.Property(p => p.NumeroInscricao)
                        .HasColumnName("Cnpj")
                        .HasMaxLength(14)
                        .IsFixedLength()
                        .IsRequired();
                });

                c.HasIndex(i => i.Chave)
                    .IsUnique();
            });

            builder.HasOne(p => p.CadastradoPor)
                .WithMany(p => p.CuponsFiscais)
                .HasForeignKey(fk => fk.CadastradoPorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.Property(p => p.CadastradoEm)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired();

            builder.Property(p => p.MensagemErro)
                .HasMaxLength(256);

            builder.ToTable("CuponsFiscais");
        }
    }
}
