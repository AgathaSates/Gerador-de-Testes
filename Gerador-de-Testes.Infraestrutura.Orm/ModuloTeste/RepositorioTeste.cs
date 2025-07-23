using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloTeste;
public class RepositorioTeste : RepositorioBaseOrm<Teste>, IRepositorioTeste
{
    public RepositorioTeste(GeradorDeTestesDbContext contexto) : base(contexto) { }

    public override Teste? SelecionarPorId(Guid idRegistro)
    {
        return _registros.Include(t => t.Disciplina)
            .Include(t => t.Questoes)
            .ThenInclude(q => q.Alternativas)
            .Include(t => t.Materias)
            .FirstOrDefault(t => t.Id.Equals(idRegistro));
    }

    public override List<Teste> SelecionarTodos() 
    {
        return _registros.Include(t => t.Disciplina)
            .Include(t => t.Questoes)
            .ThenInclude(q => q.Alternativas)
            .Include(t => t.Materias).ToList();
    }

    public Teste DuplicarTeste(Guid idTeste)
    {
        var testeOriginal = SelecionarPorId(idTeste);
        
        string novoTitulo = $"{testeOriginal.Titulo} - (Cópia)";

        var testeDuplicado = new Teste(
            novoTitulo,
            testeOriginal.Disciplina,
            testeOriginal.Materias,
            testeOriginal.QuantidadeQuestoes,
            testeOriginal.Serie,
            testeOriginal.ProvaRecuperacao,        
            testeOriginal.Questoes
        );

        return testeDuplicado;
    }
}
