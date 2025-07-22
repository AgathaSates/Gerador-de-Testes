using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloTeste;

namespace Gerador_de_Testes.Dominio.ModuloQuestao;
public class Questao : EntidadeBase<Questao>
{   
    public string Enunciado { get; set; }
    public Materia Materia { get; set; }
    public List<Alternativa> Alternativas { get; set; }
    public List<Teste> Testes { get; set; }

    public Questao()
    {
        Testes = new List<Teste>();
        Alternativas = new List<Alternativa>();
    }

    public Questao(string enunciado, Materia materia, List<Alternativa> alternativas) : this()
    {
        Id = Guid.NewGuid();
        Enunciado = enunciado;
        Materia = materia;
        Alternativas = alternativas;
    }

    public override void Atualizar(Questao registroEditado)
    {
        Enunciado = registroEditado.Enunciado;
        Materia = registroEditado.Materia;
        Alternativas = registroEditado.Alternativas;
    }

    public void AdicionarAlternativa(Alternativa alternativa)
    {
        Alternativas.Add(alternativa);
    }

    public void RemoverAlternativa(Alternativa alternativa)
    {
        Alternativas.Remove(alternativa);
    }

    public void MarcarAlternativaCorreta(Alternativa alternativa)
    {
        foreach (var a in Alternativas)
        {
            a.DesmarcarCorreta();
        }
        alternativa.MarcarCorreta();
    }
}