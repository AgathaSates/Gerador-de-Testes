using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloMateria;
public class RepositorioMateria : RepositorioBaseOrm<Materia>, IRepositorioMateria
{
    public RepositorioMateria(GeradorDeTestesDbContext contexto) : base(contexto) { }

    public override List<Materia> SelecionarTodos()
    {
        return _registros.Include(m => m.Disciplina)
            .Include(m => m.Questoes).ToList();
    }

    public override Materia? SelecionarPorId(Guid idRegistro)
    {
        return _registros.Include(m => m.Disciplina)
            .Include(m => m.Questoes)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));
    }
}