using FluentResults;
using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplina;
using Microsoft.Extensions.Logging;

namespace Gerador_de_Testes.Aplicacao.ModuloDisciplina;
public class DisciplinaService
{
    private readonly IRepositorioDisciplina _repositorio;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DisciplinaService> _logger;

    public DisciplinaService(IRepositorioDisciplina repositorio,
        IUnitOfWork unitOfWork,
        ILogger<DisciplinaService> logger)
    {
        _repositorio = repositorio;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Result<string> Cadastrar(Disciplina disciplina)
    {
        var disciplinas = _repositorio.SelecionarTodos();

        if (disciplinas.Any(i => i.Nome.Equals(disciplina.Nome)))
            return Result.Fail("Já existe uma disciplina registrada com este nome.");

        try
        {
            _repositorio.Cadastrar(disciplina);
            _unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();

            _logger.LogError(
               ex,
               "Ocorreu um erro durante o registro de {@Registro}.",
               disciplina
           );
        }

        return Result.Ok("Registro cadastrado com sucesso!");
    }
}