using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;

namespace Gerador_de_Testes.Dominio.ModuloTeste;
public class Teste : EntidadeBase<Teste>
{
    public string Titulo { get; set; }
    public int QuantidadeQuestoes { get; set; }
    public bool ProvaRecuperacao { get; set; }
    public Serie Serie { get; set; }
    public Disciplina Disciplina { get; set; }
    public List<Questao> Questoes { get; set; }
    public List<Materia> Materias { get; set; }

    public Teste()
    {
        Materias = new List<Materia>();
        Questoes = new List<Questao>();
    }

    public Teste(string titulo, Disciplina disciplina, List<Materia> materias, int quantidadeQuestoes, Serie serie, bool provaRecuperacao) : this()
    {
        Id = Guid.NewGuid();
        Titulo = titulo;
        Disciplina = disciplina;
        Materias = materias;
        QuantidadeQuestoes = quantidadeQuestoes;
        Serie = serie;
        ProvaRecuperacao = provaRecuperacao;
    }

    public void AdicionarQuestoes(List<Questao> questao)
    {
        Questoes = questao;
    }

    public override void Atualizar(Teste registroEditado)
    {
        Titulo = registroEditado.Titulo;
        Disciplina = registroEditado.Disciplina;
        Materias = registroEditado.Materias;
        QuantidadeQuestoes = registroEditado.QuantidadeQuestoes;
        Serie = registroEditado.Serie;
        ProvaRecuperacao = registroEditado.ProvaRecuperacao;
    }
}