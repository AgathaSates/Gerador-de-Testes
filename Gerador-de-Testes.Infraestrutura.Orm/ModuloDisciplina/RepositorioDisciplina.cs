using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplina;

public class RepositorioDisciplina : RepositorioBaseOrm<Disciplina>, IRepositorioDisciplina
{
    public RepositorioDisciplina(GeradorDeTestesDbContext contexto) : base(contexto) { }

    public override Disciplina? SelecionarPorId(Guid idRegistro)
    {
        return _registros
            .Include(d => d.Testes)
            .Include(d => d.Materias)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public override List<Disciplina> SelecionarTodos()
    {
        return _registros.Include(d => d.Testes).Include(d => d.Materias).ToList();
    }
}