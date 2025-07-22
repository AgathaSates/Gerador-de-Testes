using Gerador_de_Testes.Dominio.ModuloQuestao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloQuestao;
public class MapeadorQuestaoOrm : IEntityTypeConfiguration<Questao>
{
    public void Configure(EntityTypeBuilder<Questao> builder)
    {
        builder.Property(q => q.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(q => q.Enunciado)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasOne(q => q.Materia)
            .WithMany(m => m.Questoes)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(q => q.Alternativas)
            .WithOne(a => a.Questao)
            .IsRequired().OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(q => q.Testes)
            .WithMany(t => t.Questoes);
    }
}