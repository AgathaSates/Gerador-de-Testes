namespace Gerador_de_Testes.Dominio.ModuloQuestao;
public class Alternativa
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public bool Correta { get; private set; }
    public Questao Questao { get; set; }

    public Alternativa() { }
    public Alternativa(string descricao) : this()
    {
        Id = Guid.NewGuid();
        Descricao = descricao;
        Correta = false;
    }

    public void MarcarCorreta() 
    {
        Correta = true;
    }

    public void DesmarcarCorreta() 
    {
        Correta = false;
    }
}
