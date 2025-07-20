using Gerador_de_Testes.Dominio.Compartilhado;

namespace Gerador_de_Testes.Dominio.ModuloTeste;
public interface IRepositorioTeste : IRepositorio<Teste>
{
    public Teste DuplicarTeste(Guid idTeste);
}