using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Gerador_de_Testes.WebApp.Models;

public class FormularioMateriaViewModel
{
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O campo \"Serie\" é obrigatório.")]
    public Serie Serie { get; set; }
    [Required(ErrorMessage = "O campo \"Disciplina\" é obrigatório.")]
    public Guid DisciplinaId { get; set; }
    public List<SelectListItem> DisciplinasDisponiveis { get; set; }

    public FormularioMateriaViewModel()
    {
        DisciplinasDisponiveis = new();
    }
}

public class CadastrarMateriaViewModel : FormularioMateriaViewModel
{
    public CadastrarMateriaViewModel() : base()
    {
    }

    public CadastrarMateriaViewModel(List<Disciplina> disciplinas) : this()
    {
        foreach (var d in disciplinas)
        {
            var disciplinaDisponivel = new SelectListItem(d.Nome, d.Id.ToString());
            DisciplinasDisponiveis.Add(disciplinaDisponivel);
        }
    }

    public CadastrarMateriaViewModel(string nome, Serie serie, List<Disciplina> disciplinas) : this()
    {
        Nome = nome;
        Serie = serie;

        foreach (var d in disciplinas)
        {
            var disciplinaDisponivel = new SelectListItem(d.Nome, d.Id.ToString());
            DisciplinasDisponiveis.Add(disciplinaDisponivel);
        }
    }
}

public class EditarMateriaViewModel : FormularioMateriaViewModel
{
    public Guid Id { get; set; }

    public EditarMateriaViewModel() : base()
    {
    }
    public EditarMateriaViewModel(Guid id, string nome, Serie serie, List<Disciplina> disciplinas, Guid disciplinaId) : this()
    {
        Id = id;
        Nome = nome;
        Serie = serie;

        foreach (var d in disciplinas)
        {
            var disciplinaDisponivel = new SelectListItem(d.Nome, d.Id.ToString());
            DisciplinasDisponiveis.Add(disciplinaDisponivel);
        }

        DisciplinaId = disciplinaId;
    }
}

public class ExcluirMateriaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public ExcluirMateriaViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}
public class VisualizarMateriaViewModel
{
    public List<DetalhesMateriaViewModel> Materias { get; set; }
    public VisualizarMateriaViewModel(List<Materia> materias)
    {
        Materias = new();
        foreach (var m in materias)
        {
            Materias.Add(m.ParaDetalhes());
        }
    }
}
public class DetalhesMateriaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public Serie Serie { get; set; }
    public string Disciplina { get; set; }

    public DetalhesMateriaViewModel(Guid id, string nome, Serie serie, string disciplina)
    {
        Id = id;
        Nome = nome;
        Serie = serie;
        Disciplina = disciplina;
    }
}