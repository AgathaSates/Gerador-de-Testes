using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_Testes.WebApp.Extensions;
using Gerador_de_Testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerador_de_Testes.WebApp.Controllers;

[Route("questoes")]
public class QuestaoController : Controller
{
    private readonly GeradorDeTestesDbContext _contexto;
    private readonly IRepositorioQuestao _repositorioQuestao;
    private readonly IRepositorioMateria _repositorioMateria;
    private readonly IRepositorioTeste _repositorioTeste;

    public QuestaoController(GeradorDeTestesDbContext contexto, IRepositorioQuestao repositorioQuestao, 
        IRepositorioMateria repositorioMateria, IRepositorioTeste repositorioTeste)
    {
        _contexto = contexto;
        _repositorioQuestao = repositorioQuestao;
        _repositorioMateria = repositorioMateria;
        _repositorioTeste = repositorioTeste;
    }

    private List<SelectListItem> ObterMateriasSelectList()
    {
        var materias = _repositorioMateria.SelecionarTodos();
        return materias.Select(m => new SelectListItem
        {
            Text = m.Nome,
            Value = m.Id.ToString()
        }).ToList();
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Title = "Gerador de Testes | Questões";

        var questoes = _repositorioQuestao.SelecionarTodos();

        var visualizarVM = new VisualizarQuestaoViewModel(questoes);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        ViewBag.Title = "Questões | Cadastrar";

        var viewModel = new CadastrarQuestaoViewModel(ObterMateriasSelectList());

        return View(viewModel);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarQuestaoViewModel cadastrarVM)
    {
        ViewBag.Title = "Questões | Cadastrar";

        var questoes = _repositorioQuestao.SelecionarTodos();
        var materias = _repositorioMateria.SelecionarTodos();

        cadastrarVM.Materias = ObterMateriasSelectList();

        if (questoes.Any(i => i.Enunciado.Equals(cadastrarVM.Enunciado)))
        {
            ModelState.AddModelError("CadastroUnico", "Já existe uma questão registrada com este enunciado.");
            return View(cadastrarVM);
        }

        if (cadastrarVM.Alternativas.Count < 2)
        {
            ModelState.AddModelError("CadastroUnico", "É necessário cadastrar no mínimo duas alternativas.");
            return View(cadastrarVM);
        }

        if (!cadastrarVM.Alternativas.Any(a => a.Correta))
        {
            ModelState.AddModelError("CadastroUnico", "É necessário marcar uma alternativa como correta.");
            return View(cadastrarVM);
        }

        var novaQuestao = cadastrarVM.ParaEntidade(materias);

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioQuestao.Cadastrar(novaQuestao);
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
        ViewBag.Title = "Questões | Editar";

        var questaoSelecionada = _repositorioQuestao.SelecionarPorId(id);

        if (questaoSelecionada is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarQuestaoViewModel(id, questaoSelecionada!.Enunciado,
            ObterMateriasSelectList(), questaoSelecionada.Materia.Id, questaoSelecionada.Alternativas);

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarQuestaoViewModel editarVM)
    {
        ViewBag.Title = "Questões | Editar";

        editarVM.Materias = ObterMateriasSelectList();

        if (editarVM.Alternativas.Count < 2)
        {
            ModelState.AddModelError("CadastroUnico", "É necessário cadastrar no mínimo duas alternativas.");
            return View(editarVM);
        }

        if (!editarVM.Alternativas.Any(a => a.Correta))
        {
            ModelState.AddModelError("CadastroUnico", "É necessário marcar uma alternativa como correta.");
            return View(editarVM);
        }

        var materias = _repositorioMateria.SelecionarTodos();

        var questaoEditada = editarVM.ParaEntidade(materias);

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioQuestao.Editar(id, questaoEditada);
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
        ViewBag.Title = "Questões | Excluir";

        var questaoSelecionada = _repositorioQuestao.SelecionarPorId(id);

        if (questaoSelecionada is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirQuestaoViewModel(questaoSelecionada.Id, questaoSelecionada.Enunciado);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id, ExcluirQuestaoViewModel excluirVM)
    {
        ViewBag.Title = "Questões | Excluir";
        
        var testes = _repositorioTeste.SelecionarTodos();

        if (testes.Any(t => t.Questoes.Any(q => q.Id == id)))
        {
            ModelState.AddModelError("Exclusao", "Não é possível excluir uma questão que está associada a um teste.");
            excluirVM.Enunciado = _repositorioQuestao.SelecionarPorId(id)!.Enunciado;
            return View(nameof(Excluir), excluirVM);
        }

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioQuestao.Excluir(id);
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

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        ViewBag.Title = ("Questões | Detalhes");

        var questaoSelecionada = _repositorioQuestao.SelecionarPorId(id);

        if (questaoSelecionada is null)
            return RedirectToAction(nameof(Index));

        var detalhesVM = new DetalhesQuestaoViewModel(id, questaoSelecionada.Enunciado, 
            questaoSelecionada.Materia.Nome, questaoSelecionada.Alternativas);

        return View(detalhesVM);
    }

    [HttpPost("cadastrar/adicionar-alternativa")]
    [ValidateAntiForgeryToken]
    public IActionResult AdicionarAlternativa
    (CadastrarQuestaoViewModel cadastrarVM, AdicionarAlternativaViewModel addAlternatviva)
    {
        cadastrarVM.Materias = ObterMateriasSelectList();

        if (string.IsNullOrWhiteSpace(addAlternatviva.Descricao))
        {
            ModelState.AddModelError("CadastroUnico", "A descrição da alternativa não pode ser vazia.");
            return View(nameof(Cadastrar), cadastrarVM);
        }

        if (cadastrarVM.Alternativas.Any(a => a.Descricao.Equals(addAlternatviva.Descricao)))
        {
            ModelState.AddModelError("CadastroUnico", "Já existe uma alternativa com esta descrição.");
            return View(nameof(Cadastrar), cadastrarVM);
        }

        if (cadastrarVM.Alternativas.Count >= 4)
        {
            ModelState.AddModelError("CadastroUnico", "É permitido cadastrar no máximo quatro alternativas.");
            return View(nameof(Cadastrar), cadastrarVM);
        }

        cadastrarVM.AdicionarAlternativa(addAlternatviva);

        _contexto.SaveChanges();

        return View(nameof(Cadastrar), cadastrarVM);
    }

    [HttpPost("cadastrar/remover-alternativa/{alternativaId:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult RemoverAlternativa
        (Guid alternativaId, CadastrarQuestaoViewModel cadastrarVM)
    {
        cadastrarVM.Materias = ObterMateriasSelectList();

        cadastrarVM.RemoverAlternativa(alternativaId);

        _contexto.SaveChanges();

        return View(nameof(Cadastrar), cadastrarVM);
    }


    [HttpPost("cadastrar/marcar-correta/{alternativaId:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult MarcarCorreta
        (Guid alternativaId, CadastrarQuestaoViewModel cadastrarVM)
    {
        cadastrarVM.Materias = ObterMateriasSelectList();

        cadastrarVM.MarcarCorreta(alternativaId);

        _contexto.SaveChanges();

        return View(nameof(Cadastrar), cadastrarVM);
    }

    [HttpPost("editar/{questaoId:guid}/adicionar-alternativa")]
    [ValidateAntiForgeryToken]
    public IActionResult AdicionarAlternativa(Guid questaoId, EditarQuestaoViewModel editarVM,
        AdicionarAlternativaViewModel addAlternativa)
    {
        editarVM.Materias = ObterMateriasSelectList();

        if (string.IsNullOrWhiteSpace(addAlternativa.Descricao))
        {
            ModelState.AddModelError("CadastroUnico", "A descrição da alternativa não pode ser vazia.");
            return View(nameof(Editar), editarVM);
        }

        if (editarVM.Alternativas.Any(a => a.Descricao.Equals(addAlternativa.Descricao)))
        {
            ModelState.AddModelError("CadastroUnico", "Já existe uma alternativa com esta descrição.");
            return View(nameof(Editar), editarVM);
        }

        if (editarVM.Alternativas.Count >= 4)
        {
            ModelState.AddModelError("CadastroUnico", "É permitido cadastrar no máximo quatro alternativas.");
            return View(nameof(Editar), editarVM);
        }

        editarVM.Id = questaoId;

        editarVM.AdicionarAlternativa(addAlternativa);

        _contexto.SaveChanges();

        return View(nameof(Editar), editarVM);
    }

    [HttpPost("editar/{questaoId:guid}/remover-alternativa/{alternativaId:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult RemoverAlternativa(Guid questaoId, Guid alternativaId, EditarQuestaoViewModel editarVM)
    {
        editarVM.Id = questaoId;
        editarVM.Materias = ObterMateriasSelectList();

        editarVM.RemoverAlternativa(alternativaId);

        _contexto.SaveChanges();

        return View(nameof(Editar), editarVM);
    }

    [HttpPost("editar/{questaoId:guid}/marcar-correta/{alternativaId:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult MarcarCorreta(Guid questaoId, Guid alternativaId, EditarQuestaoViewModel editarVM)
    {
        editarVM.Id = questaoId;
        editarVM.Materias = ObterMateriasSelectList();

        editarVM.MarcarCorreta(alternativaId);

        _contexto.SaveChanges();

        return View(nameof(Editar), editarVM);
    }
}