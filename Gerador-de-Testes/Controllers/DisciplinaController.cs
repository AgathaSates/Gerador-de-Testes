using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_Testes.WebApp.Extensions;
using Gerador_de_Testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_Testes.WebApp.Controllers;

[Route("disciplinas")]
public class DisciplinaController : Controller
{
    private readonly GeradorDeTestesDbContext _contexto;
    private readonly IRepositorioDisciplina _repositorioDisciplina;
    private readonly IRepositorioMateria _repositorioMateria;
    private readonly IRepositorioTeste _repositorioTeste;

    public DisciplinaController(GeradorDeTestesDbContext contexto, IRepositorioDisciplina repositorioDisciplina,
        IRepositorioMateria repositorioMateria, IRepositorioTeste repositorioTeste)
    {
        _contexto = contexto;
        _repositorioDisciplina = repositorioDisciplina;
        _repositorioMateria = repositorioMateria;
        _repositorioTeste = repositorioTeste;
    }

    public IActionResult Index()
    {
        ViewBag.Title = "Gerador de Testes | Disciplinas";

        var disciplinas = _repositorioDisciplina.SelecionarTodos();
        var visualizarVM = new VisualizarDisciplinaViewModel(disciplinas);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        ViewBag.Title = "Disciplinas | Cadastrar";

        var cadastrarVM = new CadastrarDisciplinaViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarDisciplinaViewModel cadastrarVm)
    {
        ViewBag.Title = "Disciplinas | Cadastrar";

        var disciplinas = _repositorioDisciplina.SelecionarTodos();

        foreach (var item in disciplinas)
        {
            if (item.Nome.Equals(cadastrarVm.Nome))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma disciplina registrada com este nome.");
                return View(cadastrarVm);
            }
        }

        var novaDisciplina = cadastrarVm.ParaEntidade();

        var transacao = _contexto.Database.BeginTransaction();

        try 
        {
            _repositorioDisciplina.Cadastrar(novaDisciplina);
            _contexto.SaveChanges();
            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        ViewBag.Title = "Disciplinas | Editar";

        var disciplinaSelecionada = _repositorioDisciplina.SelecionarPorId(id);

        if (disciplinaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarDisciplinaViewModel(id, disciplinaSelecionada.Nome);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarDisciplinaViewModel editarVM)
    {
        ViewBag.Title = "Disciplinas | Editar";

        var disciplinas = _repositorioDisciplina.SelecionarTodos();

        if (disciplinas.Any(x => !x.Id.Equals(id) && x.Nome.Equals(editarVM.Nome)))
        {
            ModelState.AddModelError("CadastroUnico", "Já existe uma disciplina registrada com este nome.");
            return View(editarVM);
        }

        var disciplinaEditada = editarVM.ParaEntidade();

        var transacao = _contexto.Database.BeginTransaction();

        try 
        {
            _repositorioDisciplina.Editar(id, disciplinaEditada);
            _contexto.SaveChanges();
            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        ViewBag.Title = "Disciplinas | Excluir";

        var disciplinaSelecionada = _repositorioDisciplina.SelecionarPorId(id);

        if (disciplinaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirDisciplinaViewModel(disciplinaSelecionada.Id, disciplinaSelecionada.Nome);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id, ExcluirDisciplinaViewModel excluirVM)
    {
        ViewBag.Title = "Disciplinas | Excluir";

        var materias = _repositorioMateria.SelecionarTodos();
        var testes = _repositorioTeste.SelecionarTodos();

        if (materias.Any(x => x.Disciplina.Id.Equals(id)) || testes.Any(x => x.Disciplina.Id.Equals(id)))
        {
            ModelState.AddModelError("Exclusao", "Não é possível excluir esta disciplina, pois existem matérias ou testes associados a ela.");
            excluirVM.Nome = _repositorioDisciplina.SelecionarPorId(id)!.Nome;
            return View(nameof(Excluir), excluirVM);
        }

        var trasacao = _contexto.Database.BeginTransaction();

        try 
        {
            _repositorioDisciplina.Excluir(id);
            _contexto.SaveChanges();
            trasacao.Commit();
        }
        catch (Exception)
        {
            trasacao.Rollback();
            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        ViewBag.Title = "Disciplinas | Detalhes";

        var disciplinaSelecionada = _repositorioDisciplina.SelecionarPorId(id);

        if (disciplinaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var detalhesVM = new DetalhesDisciplinaViewModel(id, disciplinaSelecionada.Nome);

        return View(detalhesVM);
    }
}