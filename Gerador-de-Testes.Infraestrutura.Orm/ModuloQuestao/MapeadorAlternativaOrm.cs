using Gerador_de_Testes.Dominio.ModuloQuestao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloQuestao;
public class MapeadorAlternativaOrm : IEntityTypeConfiguration<Alternativa>
{
    public void Configure(EntityTypeBuilder<Alternativa> builder)
    {
        builder.Property(a => a.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(a => a.Descricao)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Correta)
            .IsRequired();

        builder.HasOne(a => a.Questao)
            .WithMany(q => q.Alternativas)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}