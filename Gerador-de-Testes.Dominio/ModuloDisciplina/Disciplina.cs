using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloTeste;

namespace Gerador_de_Testes.Dominio.ModuloDisciplina;

public class Disciplina : EntidadeBase<Disciplina>
{
    public string Nome { get; set; }
    public List<Teste> Testes { get; set; }
    public List<Materia> Materias { get; set; }

    public Disciplina() 
    {
        Testes = new List<Teste>();
        Materias = new List<Materia>();
    }

    public Disciplina(string nome) : this()
    {
        Id = Guid.NewGuid();
        Nome = nome;
    }

    public override void Atualizar(Disciplina registroEditado)
    {
        Nome = registroEditado.Nome;
    }
}