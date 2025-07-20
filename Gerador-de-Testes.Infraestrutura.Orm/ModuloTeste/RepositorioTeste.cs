using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloTeste;
public class RepositorioTeste : RepositorioBaseOrm<Teste>, IRepositorioTeste
{
    public RepositorioTeste(GeradorDeTestesDbContext contexto) : base(contexto) { }

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
            testeOriginal.ProvaRecuperacao        
        );

        return testeDuplicado;
    }
}
