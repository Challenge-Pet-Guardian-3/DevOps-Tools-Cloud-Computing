using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Infrastructure.Persistence.Configurations;

public sealed class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("usuario");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnName("id_usuario");

        builder.Property(u => u.Nome).HasColumnName("nome").HasMaxLength(100).IsRequired();
        builder.Property(u => u.Email).HasColumnName("email").HasMaxLength(50).IsRequired();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Senha).HasColumnName("senha").HasMaxLength(20).IsRequired();

        // 1:1 Telefone
        builder.Property(u => u.TelefoneId).HasColumnName("telefone_id_telefone").IsRequired();
        builder.HasOne(u => u.Telefone)
            .WithOne()
            .HasForeignKey<Usuario>(u => u.TelefoneId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(u => u.TelefoneId).IsUnique();

        // N:N Enderecos via UsuarioEndereco (configurado em UsuarioEnderecoConfiguration)
        // N:N Pets via UsuarioPet (configurado em UsuarioPetConfiguration)

        builder.HasMany(u => u.Tarefas)
            .WithOne(t => t.Usuario)
            .HasForeignKey(t => t.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}