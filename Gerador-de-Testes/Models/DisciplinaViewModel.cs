using System.ComponentModel.DataAnnotations;

namespace Gerador_de_Testes.WebApp.Models;

public class FormularioDisciplinaViewModel 
{
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
    [MinLength(3, ErrorMessage = "O campo \"Nome\" precisa conter ao menos 3 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Nome\" precisa conter no máximo 100 caracteres.")]
    public string Nome { get; set; }
}

public class CadastrarDisciplinaViewModel : FormularioDisciplinaViewModel
{
    public CadastrarDisciplinaViewModel() { }
    public CadastrarDisciplinaViewModel(string nome)
    {
        Nome = nome;
    }
}
public class EditarDisciplinaViewModel : FormularioDisciplinaViewModel
{
    public int Id { get; set; }
    public EditarDisciplinaViewModel() { }
    public EditarDisciplinaViewModel(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
public class ExcluirDisciplinaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public ExcluirDisciplinaViewModel() { }
    public ExcluirDisciplinaViewModel(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
public class VisualizarDisciplinaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public VisualizarDisciplinaViewModel() { }
    public VisualizarDisciplinaViewModel(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
public class DetalharDisciplinaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public DetalharDisciplinaViewModel() { }
    public DetalharDisciplinaViewModel(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
public class SelecionarDisciplinaViewModel
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public SelecionarDisciplinaViewModel() { }
    public SelecionarDisciplinaViewModel(int id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}

