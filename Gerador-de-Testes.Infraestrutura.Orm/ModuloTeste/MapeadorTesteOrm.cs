using Gerador_de_Testes.Dominio.ModuloTeste;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloTeste;
public class MapeadorTesteOrm : IEntityTypeConfiguration<Teste>
{
    public void Configure(EntityTypeBuilder<Teste> builder)
    {
        builder.Property(t => t.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(t => t.Titulo)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.QuantidadeQuestoes)
            .IsRequired();

        builder.Property(t => t.ProvaRecuperacao)
            .IsRequired();

        builder.Property(t => t.Serie)
            .IsRequired();

        builder.HasOne(t => t.Disciplina)
            .WithMany(d => d.Testes)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(t => t.Questoes)
            .WithMany(q => q.Testes);

        builder.HasMany(t => t.Materias)
            .WithMany(m => m.Testes);
    }
}