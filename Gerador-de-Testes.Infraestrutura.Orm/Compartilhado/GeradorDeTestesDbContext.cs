using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
public class GeradorDeTestesDbContext : DbContext
{
    // public DbSet<Entidade> Entidades { get; set; } *adicionar DbSet para cada entidade nova

    public DbSet<Disciplina> Disciplinas { get; set; }
    public DbSet<Materia> Materias { get; set; }

    public GeradorDeTestesDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(GeradorDeTestesDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}