using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parametriz.AutoNFP.Domain.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parametriz.AutoNFP.Data.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(pk => pk.Id);

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

            builder.ToTable("Usuarios");
        }
    }
}
