using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloQuestao;
public class RepositorioQuestao : RepositorioBaseOrm<Questao>, IRepositorioQuestao
{
    public RepositorioQuestao(GeradorDeTestesDbContext contexto) : base(contexto) { }

    public override Questao? SelecionarPorId(Guid idRegistro)
    {
        return _registros.Include(q => q.Materia)
            .Include(q => q.Testes)
            .Include(q => q.Alternativas)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));   
    }

    public override List<Questao> SelecionarTodos()
    {
        return _registros.Include(q => q.Materia)
            .Include(q => q.Testes)
            .Include(q => q.Alternativas).ToList();
    }

    public List<Questao> SelecionarQuestoesPorDisciplina(Guid disciplinaId)
    {
        return _registros
            .Where(q => q.Materia.Disciplina.Id.Equals(disciplinaId))
            .ToList();
    }

    public List<Questao> SelecionarQuestoesPorMateria(Guid materiaId)
    {
        return _registros
            .Where(q => q.Materia.Id.Equals(materiaId))
            .ToList();
    }
}