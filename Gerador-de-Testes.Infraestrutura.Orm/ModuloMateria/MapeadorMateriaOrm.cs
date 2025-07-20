using Gerador_de_Testes.Dominio.ModuloMateria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloMateria;

public class MapeadorMateriaOrm : IEntityTypeConfiguration<Materia>
{
    public void Configure(EntityTypeBuilder<Materia> builder)
    {
        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(m => m.Nome)
            .IsRequired();

        builder.Property(m => m.Serie)
            .IsRequired();

        builder.HasOne(m => m.Disciplina)
            .WithMany(d => d.Materias)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.Questoes)
            .WithOne(q => q.Materia);

        builder.HasMany(m => m.Testes)
            .WithMany(t => t.Materias);
    }
}