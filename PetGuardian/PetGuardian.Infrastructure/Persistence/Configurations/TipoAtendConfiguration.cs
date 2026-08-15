using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Infrastructure.Persistence.Configurations;

public sealed class TipoAtendConfiguration : IEntityTypeConfiguration<TipoAtend>
{
    public void Configure(EntityTypeBuilder<TipoAtend> builder)
    {
        builder.ToTable("tipo_atend");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("id_tipo_atend");

        builder.Property(t => t.Tipo)
            .HasColumnName("tipo")
            .HasMaxLength(30)
            .IsRequired();
    }
}
