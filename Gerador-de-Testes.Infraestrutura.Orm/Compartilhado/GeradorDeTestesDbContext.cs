using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
public class GeradorDeTestesDbContext : DbContext
{
    // public DbSet<Entidade> Entidades { get; set; } *adicionar DbSet para cada entidade nova

    public GeradorDeTestesDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembly = typeof(GeradorDeTestesDbContext).Assembly;

        modelBuilder.ApplyConfigurationsFromAssembly(assembly);

        base.OnModelCreating(modelBuilder);
    }
}