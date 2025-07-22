using System.ComponentModel.DataAnnotations;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerador_de_Testes.WebApp.Models;

public class FormularioQuestaoViewModel
{
    [Required(ErrorMessage = "O campo \"Enunciado\" é obrigatório.")]
    [MinLength(7, ErrorMessage = "O campo \"Enunciado\" precisa conter ao menos 7 caracteres.")]
    [MaxLength(200, ErrorMessage = "O campo \"Enunciado\" precisa conter no máximo 200 caracteres.")]
    public string Enunciado { get; set; }

    [Required(ErrorMessage = "O campo \"Matéria\" é obrigatório.")]
    public Guid MateriaId { get; set; }
    public List<SelectListItem> Materias { get; set; }
    public List<AlternativaViewModel> Alternativas { get; set; }

    public FormularioQuestaoViewModel()
    {
        Alternativas = new List<AlternativaViewModel>();
        Materias = new List<SelectListItem>();
    }

    public FormularioQuestaoViewModel(List<SelectListItem> materias) : this()
    {
        foreach (var m in materias)
        {
            Materias.Add(m);
        }
    }

    public void MarcarCorreta(Guid id)
    {
        foreach (var a in Alternativas)
            a.Correta = (a.AlternativaId == id);
    }

    public void AdicionarAlternativa(AdicionarAlternativaViewModel alternativaVM)
    {
        var alternativa = new Alternativa(alternativaVM.Descricao, alternativaVM.Correta);
        Alternativas.Add(new AlternativaViewModel(alternativa.Id, alternativa.Descricao, alternativa.Correta));
    }

    public void RemoverAlternativa(Guid id)
    {
        var alternativa = Alternativas.FirstOrDefault(a => a.AlternativaId == id);
        if (alternativa != null)
            Alternativas.Remove(alternativa);
    }
}

public class CadastrarQuestaoViewModel : FormularioQuestaoViewModel
{
    public CadastrarQuestaoViewModel() { }
    public CadastrarQuestaoViewModel(List<SelectListItem> materias) : base(materias)  { }
}

public class EditarQuestaoViewModel : FormularioQuestaoViewModel
{
    public Guid Id { get; set; }

    public EditarQuestaoViewModel() { }
    public EditarQuestaoViewModel(Guid id, string enunciado, List<SelectListItem> materias,
        Guid materiaId, List<Alternativa> alternativas) : this()
    {
        Id = id;
        Enunciado = enunciado;
        MateriaId = materiaId;

        foreach (var m in materias)
        {
            Materias.Add(m);
        }

        foreach (var a in alternativas)
        {
            Alternativas.Add(new AlternativaViewModel(a.Id, a.Descricao, a.Correta));
        }
    }
}

public class ExcluirQuestaoViewModel
{
    public Guid Id { get; set; }
    public string Enunciado { get; set; }

    public ExcluirQuestaoViewModel() { }
    public ExcluirQuestaoViewModel(Guid id, string enunciado) : this()
    {
        Id = id;
        Enunciado = enunciado;
    }
}

public class VisualizarQuestaoViewModel 
{
    public List<DetalhesQuestaoViewModel> Questoes { get; set; }

    public VisualizarQuestaoViewModel(List<Questao> questoes)
    {
        Questoes = new List<DetalhesQuestaoViewModel>();

        foreach (var q in questoes)
            Questoes.Add(q.DetalhesVM());
    }
}

public class DetalhesQuestaoViewModel
{
    public Guid Id { get; set; }
    public string Enunciado { get; set; }
    public string Materia { get; set; }
    public List<AlternativaViewModel> Alternativas { get; set; }

    public DetalhesQuestaoViewModel(Guid id, string enunciado, string materia, List<Alternativa> alternativas)
    {
        Id = id;
        Enunciado = enunciado;
        Materia = materia;
        Alternativas = new List<AlternativaViewModel>();

        foreach (var a in alternativas)
        {
            var alterantivaVM = new AlternativaViewModel(a.Id, a.Descricao, a.Correta);
            Alternativas.Add(alterantivaVM);
        }
    }
}

public class AdicionarAlternativaViewModel
{
    public string Descricao { get; set; }
    public bool Correta { get; set; }
    public AdicionarAlternativaViewModel() { }
    public AdicionarAlternativaViewModel(string descricao, bool correta)
    {
        Descricao = descricao;
        Correta = correta;
    }
}

public class AlternativaViewModel
{
    public Guid AlternativaId { get; set; }
    public string Descricao { get; set; }
    public bool Correta { get; set; }

    public AlternativaViewModel() { }
    public AlternativaViewModel(Guid id, string descricao, bool correta)
    {
        AlternativaId = id;
        Descricao = descricao;
        Correta = correta;
    }
}