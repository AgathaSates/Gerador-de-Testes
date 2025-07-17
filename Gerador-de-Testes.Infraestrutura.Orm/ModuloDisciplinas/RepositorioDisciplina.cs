using Gerador_de_Testes.Dominio.Disciplina;
using Gerador_de_Testes.Dominio.Disciplinas;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplinas
{
    public class RepositorioDisciplina : RepositorioBaseOrm<Disciplina>, IRepositorioDisciplina
    {
        public RepositorioDisciplina(ContextoDados contexto) : base(contexto) { }
        protected override List<Disciplina> ObterRegistros()
        {
            return Contexto.Disciplinas;
        }
    }

}




