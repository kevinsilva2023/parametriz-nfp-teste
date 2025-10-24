using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parametriz.AutoNFP.Domain.ErrosTransmissaoLote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Mappings
{
    public class ErroTransmissaoLoteMapping : IEntityTypeConfiguration<ErroTransmissaoLote>
    {
        public void Configure(EntityTypeBuilder<ErroTransmissaoLote> builder)
        {
            builder.HasKey(pk => pk.Id);

            builder.HasOne(p => p.Instituicao)
                .WithMany(p => p.ErrosTransmissaoLote)
                .HasForeignKey(fk => fk.InstituicaoId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(p => p.Voluntario)
                .WithMany(p => p.ErrosTransmissaoLote)
                .HasForeignKey(fk => fk.InstituicaoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.Data)
                .IsRequired();

            builder.Property(p => p.Mensagem)
                .HasMaxLength(256)
                .IsRequired();

            builder.HasIndex(i => new { i.InstituicaoId, i.VoluntarioId, i.Mensagem })
                .IsUnique();

            builder.ToTable("ErrosTransmissaoLote");
        }
    }
}
