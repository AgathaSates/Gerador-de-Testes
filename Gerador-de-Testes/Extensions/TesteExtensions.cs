using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.WebApp.Models;

namespace Gerador_de_Testes.WebApp.Extensions;

public static class TesteExtensions
{
    public static Teste ParaEntidade(this FormularioTesteViewModel formVM,
        List<Materia> materias, List<Disciplina> disciplinas, List<Questao> questoes)
    {
        var materiasSelecionadas = materias
        .Where(m => m.Id == formVM.MateriaId || formVM.ProvaRecuperacao)
        .ToList();

        var disciplinaSelecionada = disciplinas.FirstOrDefault(d => d.Id == formVM.DisciplinaId);

        var questoesSelecionadas = questoes
        .Where(q => formVM.Questoes.Any(vm => vm.Value == q.Id.ToString()))
        .ToList();

        return new Teste(formVM.Titulo, disciplinaSelecionada, materiasSelecionadas,
            formVM.QuantidadeQuestoes, formVM.Serie, formVM.ProvaRecuperacao, questoesSelecionadas);
    }

    public static DetalhesTesteViewModel DetalhesVM(this Teste teste)
    {
        return new DetalhesTesteViewModel(
            teste.Id, teste.Titulo, teste.QuantidadeQuestoes,
            teste.ProvaRecuperacao, teste.Serie, teste.Disciplina.Nome,
            teste.Materias.FirstOrDefault()?.Nome ?? "Sem matéria", teste.Questoes.Select(q => q.Enunciado).ToList()
            );
    }
}

public static class ListExtensions
{
    private static readonly Random _random = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}