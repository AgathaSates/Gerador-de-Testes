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
            .Include(m => m.Questoes)
            .Include(m => m.Testes)
            .ToList();
    }

    public override Materia? SelecionarPorId(Guid idRegistro)
    {
        return _registros.Include(m => m.Disciplina)
            .Include(m => m.Questoes)
            .Include(m => m.Testes)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public List<Materia> SelecionarMateriasPorDisciplina(Guid disciplinaId, Serie serie)
    {
        return _registros.Include(m => m.Disciplina)
            .Include(m => m.Questoes)
            .Include(m => m.Testes)
            .Where(m => m.Disciplina.Id.Equals(disciplinaId) && m.Serie == serie)
            .ToList();
    }
}