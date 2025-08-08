namespace Gerador_de_Testes.Dominio.Compartilhado;
public interface IUnitOfWork
{
    public void Commit();
    public void Rollback();
}
