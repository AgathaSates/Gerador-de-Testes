using Gerador_de_Testes.Dominio.Compartilhado;

namespace Gerador_de_Testes.Dominio.ModuloQuestao;
public interface IRepositorioQuestao : IRepositorio<Questao>
{
    public List<Questao> SelecionarQuestoesPorMateria(Guid materiaId);
    public List<Questao> SelecionarQuestoesPorDisciplina(Guid disciplinaId);
}
