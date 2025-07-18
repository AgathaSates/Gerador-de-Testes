using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.WebApp.Models;

namespace Gerador_de_Testes.WebApp.Extensions
{
    public static class DisciplinaExtensions
    {
        public static Disciplina ParaEntidade(this FormularioDisciplinaViewModel formularioVM)
        {
            return new Disciplina(formularioVM.Nome);
        }

        public static DetalhesDisciplinaViewModel ParaDetalhes(this Disciplina disciplina)
        {
            return new DetalhesDisciplinaViewModel(disciplina.Id, disciplina.Nome);              
        }
    }
}