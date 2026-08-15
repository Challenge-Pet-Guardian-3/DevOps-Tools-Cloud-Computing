using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Infrastructure.Persistence.Configurations;

public sealed class VeterinarioConfiguration : IEntityTypeConfiguration<Veterinario>
{
    public void Configure(EntityTypeBuilder<Veterinario> builder)
    {
        builder.ToTable("veterinario");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.Id).HasColumnName("id_veterinario");

        builder.Property(v => v.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
        builder.Property(v => v.Email).HasColumnName("email").HasMaxLength(50).IsRequired();
        builder.HasIndex(v => v.Email).IsUnique();
        builder.Property(v => v.Senha).HasColumnName("senha").HasMaxLength(20).IsRequired();

        builder.Property(v => v.TelefoneId).HasColumnName("telefone_id_telefone").IsRequired();
        builder.HasOne(v => v.Telefone)
            .WithOne()
            .HasForeignKey<Veterinario>(v => v.TelefoneId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(v => v.TelefoneId).IsUnique();

        // Nullable
        builder.Property(v => v.ClinicaId).HasColumnName("clinica_id_clinica").IsRequired(false);

        builder.HasMany(v => v.Atendimentos)
            .WithOne(a => a.Veterinario)
            .HasForeignKey(a => a.VeterinarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Tarefas)
            .WithOne(t => t.Veterinario)
            .HasForeignKey(t => t.VeterinarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}