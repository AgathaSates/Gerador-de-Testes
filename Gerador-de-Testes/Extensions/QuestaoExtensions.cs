using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.WebApp.Models;

namespace Gerador_de_Testes.WebApp.Extensions;

public static class QuestaoExtensions
{
    public static Questao ParaEntidade(this FormularioQuestaoViewModel formVM, List<Materia> materias)
    {
        var materiaSelecionada = 
            materias.FirstOrDefault(m => m.Id == formVM.MateriaId);

        var alternativas = formVM.Alternativas
         .Select(a => new Alternativa(a.AlternativaId, a.Descricao, a.Correta))
         .ToList();

        return new (formVM.Enunciado, materiaSelecionada!, alternativas);
    }

    public static DetalhesQuestaoViewModel DetalhesVM(this Questao questao)
    {
        return new DetalhesQuestaoViewModel(questao.Id, questao.Enunciado, questao.Materia.Nome, questao.Alternativas);
    }
}