using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloDisciplina;

namespace Gerador_de_Testes.Dominio.ModuloMateria;
public class Materia : EntidadeBase<Materia>
{
    public string Nome { get; set; }
    public Disciplina Disciplina { get; set; }
    public Serie Serie { get; set; }
    public Materia() { }

    public Materia(string nome, Disciplina disciplina, Serie serie) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Disciplina = disciplina;
        Serie = serie;
    }
    public override void Atualizar(Materia registroEditado)
    {
        Nome = registroEditado.Nome;
        Disciplina = registroEditado.Disciplina;
        Serie = registroEditado.Serie;
    }

}
