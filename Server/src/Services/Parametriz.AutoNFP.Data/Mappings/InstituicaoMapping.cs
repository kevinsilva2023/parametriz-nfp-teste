using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parametriz.AutoNFP.Domain.Instituicoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Mappings
{
    public class InstituicaoMapping : IEntityTypeConfiguration<Instituicao>
    {
        public void Configure(EntityTypeBuilder<Instituicao> builder)
        {
            builder.HasKey(pk => pk.Id);

            builder.Property(p => p.RazaoSocial)
                .HasMaxLength(256)
                .IsRequired();

            builder.OwnsOne(p => p.Cnpj, c =>
            {
                c.Ignore(i => i.TipoPessoa);

                c.Property(p => p.NumeroInscricao)
                    .HasMaxLength(14)
                    .HasColumnName("Cnpj")
                    .IsRequired();

                c.HasIndex(i => i.NumeroInscricao)
                    .IsUnique();
            });

            builder.Property(p => p.EntidadeNomeNFP)
               .HasMaxLength(256)
               .IsRequired();

            builder.OwnsOne(p => p.Endereco, c =>
            {
                c.Property(p => p.TipoLogradouro)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("TipoLogradouro");

                c.Property(p => p.Logradouro)
                    .HasMaxLength(100)
                    .HasColumnName("Logradouro");

                c.Property(p => p.Numero)
                    .HasMaxLength(10)
                    .HasColumnName("Numero");

                c.Property(p => p.Complemento)
                    .HasMaxLength(50)
                    .HasColumnName("Complemento");

                c.Property(p => p.Bairro)
                    .HasMaxLength(100)
                    .HasColumnName("Bairro");

                c.Property(p => p.CEP)
                    .HasColumnName("CEP")
                    .HasMaxLength(8)
                    .IsFixedLength();

                c.Property(p => p.Municipio)
                    .HasMaxLength(50)
                    .HasColumnName("Municipio");

                c.Property(p => p.UF)
                    .HasColumnName("UF")
                    .HasMaxLength(2)
                    .IsFixedLength();
            });

            builder.Property(p => p.Desativada)
                .IsRequired();

            builder.ToTable("Instituicoes");
        }
    }
}
