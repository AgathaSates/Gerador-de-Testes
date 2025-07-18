using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplina;
public class MapeadorDisciplinaOrm : IEntityTypeConfiguration<Disciplina>
{
    public void Configure(EntityTypeBuilder<Disciplina> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever().IsRequired();
        builder.Property(x => x.Nome)
            .HasMaxLength(100).IsRequired();
    }
}