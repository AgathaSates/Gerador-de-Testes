using Gerador_de_Testes.Dominio.Compartilhado;

namespace Gerador_de_Testes.Dominio.ModuloMateria;
public interface IRepositorioMateria : IRepositorio<Materia>
{
    public List<Materia> SelecionarMateriasPorDisciplina(Guid disciplinaId, Serie serie);
}
