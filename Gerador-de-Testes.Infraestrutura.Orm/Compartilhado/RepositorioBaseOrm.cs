using Gerador_de_Testes.Dominio.Compartilhado;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
public class RepositorioBaseOrm<T> where T: EntidadeBase<T>
{
    protected readonly DbSet<T> _registros;

    public RepositorioBaseOrm(GeradorDeTestesDbContext contexto)
    {
        _registros = contexto.Set<T>();
    }

    public void Cadastrar(T novoRegistro)
    {
        _registros.Add(novoRegistro);
    }

    public bool Editar(Guid idRegistro, T registroEditado)
    {
        var registroSelecionado = SelecionarPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.Atualizar(registroEditado);

        return true;
    }

    public bool Excluir(Guid idRegistro)
    {
        var registroSelecionado = SelecionarPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        _registros.Remove(registroSelecionado);

        return true;
    }

    public virtual T? SelecionarPorId(Guid idRegistro)
    {
        return _registros.FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public virtual List<T> SelecionarTodos()
    {
        return _registros.ToList();
    }
}