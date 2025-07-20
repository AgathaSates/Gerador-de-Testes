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
            .Include(q => q.Testes).FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public override List<Questao> SelecionarTodos()
    {
        return _registros.Include(q => q.Materia).Include(q => q.Testes).ToList();
    }

    //Para cadastro de testes
    public List<Questao> SelecionarQuestoesPorDisciplina(Guid disciplinaId, Serie serieEscolar)
    {
        return _registros
            .Where(q => q.Materia.Disciplina.Id.Equals(disciplinaId) && q.Materia.Serie == serieEscolar)
            .ToList();
    }

    public List<Questao> SelecionarQuestoesPorMateria(Guid materiaId, Serie serieEscolar)
    {
        return _registros
            .Where(q => q.Materia.Id.Equals(materiaId) && q.Materia.Serie == serieEscolar)
            .ToList();
    }

    public List<Questao> SortearQuestoes(Guid disciplinaId, Guid? materiaId,
        Serie serie, int quantidade, bool provaRecuperacao)
    {
        return _registros.Where(q => q.Materia.Disciplina.Id == disciplinaId &&
                q.Materia.Serie == serie && (provaRecuperacao || q.Materia.Id == materiaId))
     .OrderBy(q => Guid.NewGuid()).Take(quantidade) .ToList();
    }
}