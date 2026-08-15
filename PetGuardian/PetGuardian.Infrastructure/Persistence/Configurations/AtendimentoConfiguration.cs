using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetGuardian.Domain.Entities;

namespace PetGuardian.Infrastructure.Persistence.Configurations;

public sealed class AtendimentoConfiguration : IEntityTypeConfiguration<Atendimento>
{
    public void Configure(EntityTypeBuilder<Atendimento> builder)
    {
        builder.ToTable("atendimento");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id_atendimento");

        builder.Property(a => a.Data).HasColumnName("data").IsRequired();
        builder.Property(a => a.Anotacoes).HasColumnName("anotacoes").HasMaxLength(300).IsRequired();
        builder.Property(a => a.Valor).HasColumnName("valor").HasColumnType("NUMBER(10,2)").IsRequired();

        builder.Property(a => a.PetId).HasColumnName("pet_id_pet").IsRequired();
        builder.HasOne(a => a.Pet)
            .WithMany(p => p.Atendimentos)
            .HasForeignKey(a => a.PetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.VeterinarioId).HasColumnName("veterinario_id_veterinario").IsRequired();

        builder.Property(a => a.TipoAtendId).HasColumnName("tipo_atend_id_tipo_atend").IsRequired();
        builder.HasOne(a => a.TipoAtend)
            .WithMany(t => t.Atendimentos)
            .HasForeignKey(a => a.TipoAtendId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(a => a.StatusId).HasColumnName("status_id_status").IsRequired();
        builder.HasOne(a => a.Status)
            .WithMany(s => s.Atendimentos)
            .HasForeignKey(a => a.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}