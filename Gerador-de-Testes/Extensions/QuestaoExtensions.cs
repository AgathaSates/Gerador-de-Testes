using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.WebApp.Models;

namespace Gerador_de_Testes.WebApp.Extensions;

public static class QuestaoExtensions
{
    public static Questao ParaEntidade(this FormularioQuestaoViewModel formVM, List<Materia> materias)
    {
        Materia? materiaSelecionada = null;

        foreach (var m in materias)
        {
            if (m.Id == formVM.MateriaId)
                materiaSelecionada = m;
        }

        List<Alternativa> alternativas = new List<Alternativa>();

        foreach (var a in formVM.Alternativas)
        {
            alternativas.Add(new Alternativa(a.AlternativaId, a.Descricao, a.Correta));
        }

        return new Questao(formVM.Enunciado, materiaSelecionada!, alternativas);
    }

    public static DetalhesQuestaoViewModel DetalhesVM(this Questao questao)
    {
        return new DetalhesQuestaoViewModel(questao.Id, questao.Enunciado, questao.Materia.Nome, questao.Alternativas);
    }
}