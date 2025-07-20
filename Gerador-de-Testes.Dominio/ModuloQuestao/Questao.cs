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

    public Questao(Materia materia, string enunciado) : this()
    {
        Id = Guid.NewGuid();
        Materia = materia;
        Enunciado = enunciado;
    }

    public override void Atualizar(Questao registroEditado)
    {
        Materia = registroEditado.Materia;
        Enunciado = registroEditado.Enunciado;
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