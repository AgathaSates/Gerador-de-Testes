using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.Dominio.ModuloTeste;

namespace Gerador_de_Testes.Dominio.ModuloMateria;
public class Materia : EntidadeBase<Materia>
{
    public string Nome { get; set; }
    public Serie Serie { get; set; }
    public Disciplina Disciplina { get; set; }
    public List<Questao> Questoes { get; set; }
    public List<Teste> Testes { get; set; }

    public Materia() 
    {
        Testes = new List<Teste>();
        Questoes = new List<Questao>();
    }

    public Materia(string nome, Serie serie, Disciplina disciplina) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Serie = serie;
        Disciplina = disciplina;
    }
    public override void Atualizar(Materia registroEditado)
    {
        Nome = registroEditado.Nome;
        Disciplina = registroEditado.Disciplina;
        Serie = registroEditado.Serie;
    }
}
