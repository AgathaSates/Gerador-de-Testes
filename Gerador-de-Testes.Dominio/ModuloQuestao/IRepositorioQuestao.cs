using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloMateria;

namespace Gerador_de_Testes.Dominio.ModuloQuestao;
public interface IRepositorioQuestao : IRepositorio<Questao>
{
    public List<Questao> SelecionarQuestoesPorMateria(Guid materiaId, Serie serie);
    public List<Questao> SelecionarQuestoesPorDisciplina(Guid disciplinaId, Serie serie);
}
