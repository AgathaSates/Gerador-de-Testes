using Gerador_de_Testes.Dominio.ModuloDisciplinas;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplinas
{
    public class RepositorioDisciplina : RepositorioBaseOrm<Disciplina>, IRepositorioDisciplina
    {
        public RepositorioDisciplina(GeradorDeTestesDbContext contexto) : base(contexto) { }
    }
}




