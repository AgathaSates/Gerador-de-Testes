using System.ComponentModel.DataAnnotations;
using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.WebApp.Extensions;

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
    public Guid Id { get; set; }
    public EditarDisciplinaViewModel() { }
    public EditarDisciplinaViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}

public class ExcluirDisciplinaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public ExcluirDisciplinaViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarDisciplinaViewModel
{
    public List<DetalhesDisciplinaViewModel> Disciplinas { get; set; }

    public VisualizarDisciplinaViewModel(List<Disciplina> disciplinas)
    {
        Disciplinas = new List<DetalhesDisciplinaViewModel>();

        foreach (var d in disciplinas)
        {
            Disciplinas.Add(d.ParaDetalhes());
        }
    }
}

public class DetalhesDisciplinaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
   
    public DetalhesDisciplinaViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}