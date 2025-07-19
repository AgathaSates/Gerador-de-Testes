using Gerador_de_Testes.Dominio.ModuloMateria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloMateria;

public class MapeadorMateriaOrm : IEntityTypeConfiguration<Materia>
{
    public void Configure(EntityTypeBuilder<Materia> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever().IsRequired();
        builder.Property(x => x.Nome).IsRequired();
        builder.HasOne(d => d.Disciplina).WithMany().IsRequired();
        builder.Property(x => x.Serie).IsRequired();
    }
}
