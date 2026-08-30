using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetGuardian.Domain.Entities;
using PetGuardian.Domain.Enums;

namespace PetGuardian.Infrastructure.Persistence.Configurations;

/// <summary>
/// "senha" cresceu de VARCHAR2(20) para VARCHAR2(60); adicionada "role"
/// (CHECK IN ('COMUM','PREMIUM')) e a UNIQUE constraint explícita em email (já era única via índice).
/// Relação com Tarefas segue Restrict (FK usuario_id_usuario é NOT NULL desde a sprint anterior).
/// </summary>
public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id_usuario");
        builder.Property(u => u.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(50).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique(); // reflete a constraint usuario_email_UN do banco

        builder.Property(u => u.Senha).HasColumnName("senha").HasMaxLength(60).IsRequired();

        // CHECK: role IN ('COMUM', 'PREMIUM')
        builder.Property(u => u.Role)
            .HasColumnName("role")
            .HasMaxLength(10)
            .HasConversion(
                v => v.ToString().ToUpperInvariant(),
                v => Enum.Parse<RoleUsuario>(v, true))
            .IsRequired();

        builder.Property(u => u.TelefoneId).HasColumnName("telefone_id_telefone").IsRequired();
        builder.HasOne(u => u.Telefone)
            .WithOne()
            .HasForeignKey<Usuario>(u => u.TelefoneId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(u => u.TelefoneId).IsUnique();

        builder.HasMany(u => u.Tarefas)
            .WithOne(t => t.Usuario)
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}