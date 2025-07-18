using Gerador_de_Testes.Dominio.Disciplina;
using Gerador_de_Testes.WebApp.Models;

namespace Gerador_de_Testes.WebApp.Extensions
{
    public static class DisciplinaExtensions
    {
        public static Disciplina ParaEntidade(this FormularioDisciplinaViewModel formularioVM)
        {
            return new Disciplina(nome: formularioVM.Nome);
        }
        public static CadastrarDisciplinaViewModel ParaCadastrarVM(this Disciplina disciplina)
        {
            return new CadastrarDisciplinaViewModel(disciplina.Nome);
        }
    }
}

