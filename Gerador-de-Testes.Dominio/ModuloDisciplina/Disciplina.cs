using Gerador_de_Testes.Dominio.Compartilhado;

namespace Gerador_de_Testes.Dominio.ModuloDisciplina;

public class Disciplina : EntidadeBase<Disciplina>
{
    public string Nome { get; set; }

    public Disciplina() { }

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