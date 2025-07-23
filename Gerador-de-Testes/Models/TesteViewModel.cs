using System.ComponentModel.DataAnnotations;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerador_de_Testes.WebApp.Models;

public class FormularioTesteViewModel
{
    [Required(ErrorMessage = "O campo \"Titulo\" é obrigatório.")]
    [MinLength(7, ErrorMessage = "O campo \"Titulo\" precisa conter ao menos 7 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Titulo\" precisa conter no máximo 100 caracteres.")]
    public string Titulo { get; set; }

    [Range(1, 20, ErrorMessage = "Informe um valor entre 1 e 20.")]
    public int QuantidadeQuestoes { get; set; }

    [Display(Name = "Prova de Recuperação")]
    public bool ProvaRecuperacao { get; set; }

    [Required(ErrorMessage = "O campo \"Série\" é obrigatório.")]
    public Serie Serie { get; set; }
    public Guid DisciplinaId { get; set; }
    public string DisciplinaNome { get; set; }
    public Guid MateriaId { get; set; }
    public string MateriaNome { get; set; }
    public List<SelectListItem> Questoes { get; set; }
    public List<SelectListItem> Materias { get; set; }
    public List<SelectListItem> Disciplinas { get; set; }

    public FormularioTesteViewModel()
    {
        Questoes = new List<SelectListItem>();
        Materias = new List<SelectListItem>();
        Disciplinas = new List<SelectListItem>();
    }

    public FormularioTesteViewModel(List<SelectListItem> disciplinas) : this()
    {
        Disciplinas.AddRange(disciplinas);
    }
}

public class CadastrarTesteViewModel : FormularioTesteViewModel
{
    public CadastrarTesteViewModel() { }

    public CadastrarTesteViewModel(List<SelectListItem> disciplinas) : base(disciplinas) { }
}

public class DuplicarTesteViewModel : FormularioTesteViewModel
{
    public DuplicarTesteViewModel() { }
    public DuplicarTesteViewModel(Teste teste) 
    {
        Titulo = teste.Titulo;
        QuantidadeQuestoes = teste.QuantidadeQuestoes;
        ProvaRecuperacao = teste.ProvaRecuperacao;
        Serie = teste.Serie;
        DisciplinaId = teste.Disciplina.Id;
        DisciplinaNome = teste.Disciplina.Nome;
        if (!teste.ProvaRecuperacao)
        {
            MateriaId = teste.Materias.FirstOrDefault()?.Id ?? Guid.Empty;
            MateriaNome = teste.Materias.FirstOrDefault()?.Nome ?? "Matéria não cadastrada";
        }
    }
}

public class ExcluirTesteViewModel 
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }

    public ExcluirTesteViewModel() { }

    public ExcluirTesteViewModel(Guid id, string titulo)
    {
        Id = id;
        Titulo = titulo;
    }
}

public class VisualizarTesteViewModel
{
    public List<DetalhesTesteViewModel> Testes { get; set; }
    public VisualizarTesteViewModel(List<Teste> testes)
    {
        Testes = testes.Select(t => t.DetalhesVM()).ToList();
    }
}

public class DetalhesTesteViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; }
    public int QuantidadeQuestoes { get; set; }
    public bool ProvaRecuperacao { get; set; }
    public Serie Serie { get; set; }
    public string Disciplina { get; set; }
    public string Materia { get; set; }
    public List<string> Questoes { get; set; }

    public DetalhesTesteViewModel(Guid id, string titulo, int quantidadequestoes, 
        bool recuperacao, Serie serie, string disciplina, string materia, List<string> questoes)
    {
        Id = id;
        Titulo = titulo;
        QuantidadeQuestoes = quantidadequestoes;
        ProvaRecuperacao = recuperacao;
        Serie = serie;
        Disciplina = disciplina;
        Materia = materia;
        Questoes = questoes;
    }
}