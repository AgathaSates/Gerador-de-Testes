using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_Testes.WebApp.Extensions;
using Gerador_de_Testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_Testes.WebApp.Controllers;

[Route("materias")]
public class MateriaController : Controller
{
    private readonly GeradorDeTestesDbContext _contexto;
    private readonly IRepositorioMateria _repositorioMateria;
    private readonly IRepositorioDisciplina _repositorioDisciplina;
    private readonly IRepositorioQuestao _repositorioQuestao;

    public MateriaController(GeradorDeTestesDbContext contexto, IRepositorioMateria repositorioMateria, 
        IRepositorioDisciplina repositorioDisciplina, IRepositorioQuestao repositorioQuestao)
    {
        _contexto = contexto;
        _repositorioMateria = repositorioMateria;
        _repositorioDisciplina = repositorioDisciplina;
        _repositorioQuestao = repositorioQuestao;
    }

    public IActionResult Index()
    {
        ViewBag.Title = "Gerador de Testes | Materias";

        var visualizarVm= new VisualizarMateriaViewModel(_repositorioMateria.SelecionarTodos());

        return View(visualizarVm);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        ViewBag.Title = "Materias | Cadastrar";

        var disciplinas = _repositorioDisciplina.SelecionarTodos();

        var cadastrarVM = new CadastrarMateriaViewModel(disciplinas);

        return View(cadastrarVM);
    }


    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarMateriaViewModel cadastrarVm)
    {
        ViewBag.Title = "Materias | Cadastrar";

        var materias = _repositorioMateria.SelecionarTodos();
        var disciplinas = _repositorioDisciplina.SelecionarTodos();

        foreach (var item in materias)
        {
            if (item.Nome.Equals(cadastrarVm.Nome))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma materia registrada com este nome.");
                return View(cadastrarVm);
            }
        }

        var novaDisciplina = cadastrarVm.ParaEntidade(disciplinas);

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioMateria.Cadastrar(novaDisciplina);
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
        ViewBag.Title = "Materias | Editar";

        var materiaSelecionada = _repositorioMateria.SelecionarPorId(id);
        var disciplinas = _repositorioDisciplina.SelecionarTodos();

        if (materiaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarMateriaViewModel(id, materiaSelecionada.Nome, materiaSelecionada.Serie, disciplinas, materiaSelecionada.Disciplina.Id);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarMateriaViewModel editarVM)
    {
        var materias = _repositorioMateria.SelecionarTodos();
        var disciplinas = _repositorioDisciplina.SelecionarTodos();

        if (materias.Any(x => !x.Id.Equals(id) && x.Nome.Equals(editarVM.Nome)))
        {
            ModelState.AddModelError("CadastroUnico", "Já existe uma disciplina registrada com este nome.");
            return View(editarVM);
        }

        var materiaEditada = editarVM.ParaEntidade(disciplinas);

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioMateria.Editar(id, materiaEditada);
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
        ViewBag.Title = "Materias | Excluir";

        var materiaSelecionada = _repositorioMateria.SelecionarPorId(id);

        if (materiaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirMateriaViewModel(materiaSelecionada.Id, materiaSelecionada.Nome);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id, ExcluirMateriaViewModel excluirVM)
    {
        var questoes = _repositorioQuestao.SelecionarTodos();

        if (questoes.Any(x => x.Materia.Id.Equals(id)))
        {
            ModelState.AddModelError("Exclusao", "Não é possível excluir esta matéria, pois existem questões associadas a ela.");
            excluirVM.Nome = _repositorioMateria.SelecionarPorId(id)!.Nome;
            return View(nameof(Excluir), excluirVM);
        }

        var trasacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioMateria.Excluir(id);
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
        ViewBag.Title = ("Materias | Detalhes");

        var materiaSelecionada = _repositorioMateria.SelecionarPorId(id);

        if (materiaSelecionada is null)
            return RedirectToAction(nameof(Index));


        var detalhesVM = new DetalhesMateriaViewModel(id, materiaSelecionada.Nome, materiaSelecionada.Serie, materiaSelecionada.Disciplina.Nome);

        return View(detalhesVM);
    }
}