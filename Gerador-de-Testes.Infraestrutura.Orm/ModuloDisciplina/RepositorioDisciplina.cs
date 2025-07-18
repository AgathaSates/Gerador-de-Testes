using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;

namespace Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplina;

public class RepositorioDisciplina : RepositorioBaseOrm<Disciplina>, IRepositorioDisciplina
{
    public RepositorioDisciplina(GeradorDeTestesDbContext contexto) : base(contexto) { }
}