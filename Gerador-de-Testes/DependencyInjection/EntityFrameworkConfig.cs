using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.WebApp.DependencyInjection;

public static class EntityFrameworkConfig
{
    public static void AddEntityFrameworkConfig(this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration["SQL_CONNECTION_STRING"];

        services.AddDbContext<IUnitOfWork, GeradorDeTestesDbContext>(options =>
            options.UseSqlServer(connectionString));
    }
}