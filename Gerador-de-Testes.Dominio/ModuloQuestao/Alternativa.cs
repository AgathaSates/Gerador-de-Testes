namespace Gerador_de_Testes.Dominio.ModuloQuestao;
public class Alternativa
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public bool Correta { get; private set; }

    public Questao Questao { get; set; }

    public Alternativa(Guid id, string descricao, bool correta)
    {
        Id = id;
        Descricao = descricao;
        Correta = correta;
    }

    public Alternativa(string descricao)
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
