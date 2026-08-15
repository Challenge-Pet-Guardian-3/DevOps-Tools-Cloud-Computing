using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Infrastructure.Persistence.Configurations;

public sealed class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("tarefa");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id_tarefa");

        builder.Property(t => t.Titulo).HasColumnName("titulo").HasMaxLength(30).IsRequired();
        builder.Property(t => t.PontosTarefa).HasColumnName("pontos_tarefa").HasColumnType("NUMBER(3)").IsRequired();
        builder.Property(t => t.Descricao).HasColumnName("descricao").HasMaxLength(200).IsRequired();
        builder.Property(t => t.Criacao).HasColumnName("criacao").IsRequired();
        builder.Property(t => t.Prazo).HasColumnName("prazo").IsRequired();
        builder.Property(t => t.Conclusao).HasColumnName("conclusao").IsRequired(false);

        // Nullable — usuário pode ser atribuído depois
        builder.Property(t => t.UsuarioId).HasColumnName("usuario_id_usuario").IsRequired(false);
        builder.HasIndex(t => t.UsuarioId);

        builder.Property(t => t.PetId).HasColumnName("pet_id_pet").IsRequired();
        builder.HasOne(t => t.Pet)
            .WithMany(p => p.Tarefas)
            .HasForeignKey(t => t.PetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(t => t.StatusId).HasColumnName("status_id_status").IsRequired();
        builder.HasOne(t => t.Status)
            .WithMany(s => s.Tarefas)
            .HasForeignKey(t => t.StatusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.VeterinarioId).HasColumnName("veterinario_id_veterinario").IsRequired();
    }
}
