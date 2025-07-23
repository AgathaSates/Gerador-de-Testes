using Gerador_de_Testes.Dominio.ModuloDisciplina;
using Gerador_de_Testes.Dominio.ModuloMateria;
using Gerador_de_Testes.Dominio.ModuloQuestao;
using Gerador_de_Testes.Dominio.ModuloTeste;
using Gerador_de_Testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_Testes.WebApp.Extensions;
using Gerador_de_Testes.WebApp.Models;
using Gerador_de_Testes.WebApp.PDF;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using QuestPDF.Fluent;

namespace Gerador_de_Testes.WebApp.Controllers;

[Route("testes")]
public class TesteController : Controller
{
    private readonly GeradorDeTestesDbContext _contexto;
    private readonly IRepositorioTeste _repositorioTeste;
    private readonly IRepositorioMateria _repositorioMateria;
    private readonly IRepositorioDisciplina _repositorioDisciplina;
    private readonly IRepositorioQuestao _repositorioQuestao;

    public TesteController(
        GeradorDeTestesDbContext contexto,
        IRepositorioTeste repositorioTeste,
        IRepositorioMateria repositorioMateria,
        IRepositorioDisciplina repositorioDisciplina,
        IRepositorioQuestao repositorioQuestao)
    {
        _contexto = contexto;
        _repositorioTeste = repositorioTeste;
        _repositorioMateria = repositorioMateria;
        _repositorioDisciplina = repositorioDisciplina;
        _repositorioQuestao = repositorioQuestao;
    }
    private List<SelectListItem> ObterSelectListDeDisciplinas()
    {
        return _repositorioDisciplina.SelecionarTodos()
            .Select(d => new SelectListItem { Text = d.Nome, Value = d.Id.ToString() })
            .ToList();
    }

    private List<SelectListItem> ObterSelectListDeMaterias(bool isRecuperacao, Guid disciplinaId = default)
    {
        var materias = isRecuperacao
            ? _repositorioMateria.SelecionarMateriasPorDisciplina(disciplinaId)
            : _repositorioMateria.SelecionarTodos();

        return materias
            .Select(m => new SelectListItem { Text = m.Nome, Value = m.Id.ToString() })
            .ToList();
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Title = "Gerador de Testes | Testes";

        var testes = _repositorioTeste.SelecionarTodos();

        var visualizarVM = new VisualizarTesteViewModel(testes);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        ViewBag.Title = "Testes | Cadastrar";

        var cadastrarVM = new CadastrarTesteViewModel(
            ObterSelectListDeMaterias(false), ObterSelectListDeDisciplinas());

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarTesteViewModel cadastrarVM)
    {
        ViewBag.Title = "Testes | Cadastrar";

        var questões = _repositorioQuestao.SelecionarQuestoesPorDisciplina(cadastrarVM.DisciplinaId);
        var testes = _repositorioTeste.SelecionarTodos();

        if (cadastrarVM.QuantidadeQuestoes > questões.Count)
        {
            ModelState.AddModelError("CadastroUnico", $"A quantidade de questões informada deve ser menor ou igual a quantidade de questões cadastradas. >> Quantidade Atual: {questões.Count}");
            cadastrarVM.Disciplinas = ObterSelectListDeDisciplinas();
            return View(cadastrarVM);
        }

        if (testes.Any(t => t.Titulo.Equals(cadastrarVM.Titulo, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError("CadastroUnico", "Já existe um teste cadastrado com o mesmo nome.");
            cadastrarVM.Disciplinas = ObterSelectListDeDisciplinas();
            return View(cadastrarVM);
        }

        var json = JsonConvert.SerializeObject(cadastrarVM);
        TempData["CadastroParcial"] = json;

        return RedirectToAction(nameof(CadastrarEtapa2));
    }

    [HttpGet("cadastrarEtapa2")]
    public IActionResult CadastrarEtapa2()
    {
        ViewBag.Title = "Testes | Cadastrar";

        var json = TempData["CadastroParcial"] as string;

        if (string.IsNullOrEmpty(json))
            return RedirectToAction("Cadastrar");

        var vmParcial = JsonConvert.DeserializeObject<CadastrarTesteViewModel>(json);

        vmParcial!.DisciplinaNome = _repositorioDisciplina.SelecionarPorId(vmParcial.DisciplinaId)!.Nome;

        vmParcial.Materias = vmParcial.ProvaRecuperacao
            ? ObterSelectListDeMaterias(true, vmParcial.DisciplinaId)
            : ObterSelectListDeMaterias(false);

        TempData.Keep("CadastroParcial");

        return View(vmParcial);
    }

    [HttpPost("cadastrarEtapa2")]
    [ValidateAntiForgeryToken]
    public IActionResult CadastrarEtapa2(CadastrarTesteViewModel cadastrarVM)
    {
        ViewBag.Title = "Testes | Cadastrar";

        var materias = _repositorioMateria.SelecionarTodos();
        var disciplinas = _repositorioDisciplina.SelecionarTodos();
        var questoes = _repositorioQuestao.SelecionarTodos();

        var novoTeste = cadastrarVM.ParaEntidade(materias, disciplinas, questoes);

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioTeste.Cadastrar(novoTeste);
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

    [HttpPost("cadastrar/sortear")]
    [ValidateAntiForgeryToken]
    public IActionResult Sortear(CadastrarTesteViewModel cadastrarVM)
    {
        var json = TempData["CadastroParcial"] as string;

        if (string.IsNullOrEmpty(json))
            return RedirectToAction("Cadastrar");

        var vmParcial = JsonConvert.DeserializeObject<CadastrarTesteViewModel>(json);

        vmParcial!.DisciplinaNome = _repositorioDisciplina.SelecionarPorId(vmParcial.DisciplinaId)!.Nome;
        vmParcial.MateriaId = cadastrarVM.MateriaId;

        var materia = _repositorioMateria.SelecionarPorId(vmParcial.MateriaId);

        vmParcial.Materias = vmParcial.ProvaRecuperacao
            ? ObterSelectListDeMaterias(true, vmParcial.DisciplinaId)
            : ObterSelectListDeMaterias(false);

        if (!vmParcial.ProvaRecuperacao && vmParcial.QuantidadeQuestoes > materia.Questoes.Count)
        {
            ModelState.AddModelError("CadastroUnico", $"Não há questões suficientes na matéria selecionada, selecione um número de questões disponiveis. >> Quantidade de questões disponiveis: {materia.Questoes.Count}");

            TempData["CadastroParcial"] = JsonConvert.SerializeObject(vmParcial);
            TempData.Keep("CadastroParcial");

            return View("CadastrarEtapa2", vmParcial);
        }

        var questoes = vmParcial.ProvaRecuperacao
              ? _repositorioQuestao.SelecionarQuestoesPorDisciplina(vmParcial.DisciplinaId)
              : _repositorioQuestao.SelecionarQuestoesPorMateria(vmParcial.MateriaId);

        var questoesList = questoes.Select(q => new SelectListItem
        {
            Text = q.Enunciado,
            Value = q.Id.ToString()
        }).ToList();

        questoesList.Shuffle();

        vmParcial.Questoes = questoesList.Take(vmParcial.QuantidadeQuestoes).ToList();

        TempData.Keep("CadastroParcial");

        return View("CadastrarEtapa2", vmParcial);
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        ViewBag.Title = ("Testes | Detalhes");

        var testeSelecionado = _repositorioTeste.SelecionarPorId(id);

        if (testeSelecionado is null)
            return RedirectToAction(nameof(Index));

        var nomeMateria = testeSelecionado.Materias
             .Select(m => m.Nome)
             .FirstOrDefault() ?? "Sem matéria";
        var enunciados = testeSelecionado.Questoes
              .Select(q => q.Enunciado)
              .ToList();

        var detalhesVM = new DetalhesTesteViewModel(testeSelecionado.Id,
            testeSelecionado.Titulo, testeSelecionado.QuantidadeQuestoes,
            testeSelecionado.ProvaRecuperacao, testeSelecionado.Serie,
            testeSelecionado.Disciplina.Nome, nomeMateria, enunciados);

        return View(detalhesVM);
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        ViewBag.Title = "Testes | Excluir";

        var testeSelecionado = _repositorioTeste.SelecionarPorId(id);

        if (testeSelecionado is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirTesteViewModel(testeSelecionado.Id, testeSelecionado.Titulo);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id, ExcluirQuestaoViewModel excluirVM)
    {
        ViewBag.Title = "Testes | Excluir";

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioTeste.Excluir(id);
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

    [HttpGet("duplicar/{id:guid}")]
    public IActionResult Duplicar(Guid id)
    {
        ViewBag.Title = "Testes | Duplicar";

        var testeduplicado = _repositorioTeste.DuplicarTeste(id);
        var duplicarVM = new DuplicarTesteViewModel(testeduplicado); ;

        duplicarVM.MateriaNome = testeduplicado.Materias.FirstOrDefault()?.Nome!;

        TempData["DuplicarParcial"] = JsonConvert.SerializeObject(duplicarVM);
        TempData.Keep("DuplicarParcial");

        return View(duplicarVM);
    }

    [HttpPost("duplicar")]
    [ValidateAntiForgeryToken]
    public IActionResult Duplicar(DuplicarTesteViewModel duplicarVM)
    {
        var json = TempData["DuplicarParcial"] as string;

        if (string.IsNullOrEmpty(json))
            return RedirectToAction("Index");

        var vmParcial = JsonConvert.DeserializeObject<DuplicarTesteViewModel>(json);

        vmParcial.Titulo = duplicarVM.Titulo;

        var testes = _repositorioTeste.SelecionarTodos();

        if (testes.Any(t => t.Titulo.Equals(vmParcial.Titulo, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError("CadastroUnico", "Já existe um teste cadastrado com o mesmo nome.");
            TempData["DuplicarParcial"] = JsonConvert.SerializeObject(vmParcial);
            TempData.Keep("DuplicarParcial");
            return View("Duplicar", vmParcial);
        }

        var disciplinas = _repositorioDisciplina.SelecionarTodos();
        var materias = _repositorioMateria.SelecionarTodos();
        var questoes = _repositorioQuestao.SelecionarTodos();

        var testeDuplicado = vmParcial.ParaEntidade(materias, disciplinas, questoes);

        var transacao = _contexto.Database.BeginTransaction();

        try
        {
            _repositorioTeste.Cadastrar(testeDuplicado);
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


    [HttpPost("duplicar/sortear")]
    [ValidateAntiForgeryToken]
    public IActionResult Sortear(DuplicarTesteViewModel duplicarVM)
    {
        var json = TempData["DuplicarParcial"] as string;
        if (string.IsNullOrEmpty(json))
            return RedirectToAction("Duplicar");

        var vmParcial = JsonConvert.DeserializeObject<DuplicarTesteViewModel>(json);

        var materia = _repositorioMateria.SelecionarPorId(vmParcial.MateriaId);


        var questoes = vmParcial.ProvaRecuperacao
              ? _repositorioQuestao.SelecionarQuestoesPorDisciplina(vmParcial.DisciplinaId)
              : _repositorioQuestao.SelecionarQuestoesPorMateria(vmParcial.MateriaId);


        if (!vmParcial.ProvaRecuperacao && vmParcial.QuantidadeQuestoes > materia.Questoes.Count)
        {
            ModelState.AddModelError("CadastroUnico", $"Não há questões suficientes na matéria selecionada, selecione um número de questões disponiveis. >> Quantidade de questões disponiveis: {materia.Questoes.Count}");

            TempData["DuplicarParcial"] = JsonConvert.SerializeObject(vmParcial);
            TempData.Keep("DuplicarParcial");
            return View("Duplicar", vmParcial);
        }

        if (vmParcial.QuantidadeQuestoes > questoes.Count)
        {
            ModelState.AddModelError("CadastroUnico", $"A quantidade de questões informada deve ser menor ou igual a quantidade de questões cadastradas. >> Quantidade Atual: {questoes.Count}");
            TempData["DuplicarParcial"] = JsonConvert.SerializeObject(vmParcial);
            TempData.Keep("DuplicarParcial");
            return View("Duplicar", vmParcial);
        }

        var questoesList = questoes.Select(q => new SelectListItem
        {
            Text = q.Enunciado,
            Value = q.Id.ToString()
        }).ToList();

        questoesList.Shuffle();

        vmParcial.Questoes = questoesList.Take(vmParcial.QuantidadeQuestoes).ToList();

        TempData["DuplicarParcial"] = JsonConvert.SerializeObject(vmParcial);
        TempData.Keep("DuplicarParcial");

        return View("Duplicar", vmParcial);
    }

    [HttpGet("gerar-pdf/{id}")]
    public IActionResult GerarPdf(Guid id)
    {
        var teste = _repositorioTeste.SelecionarPorId(id);

        if (teste == null)
            return NotFound();

        var questoes = teste.Questoes.Select(q => new TestePdfDocument.QuestaoDto
        {
            Enunciado = q.Enunciado,

            Alternativas = q.Alternativas
                .Select((a, index) => new TestePdfDocument.AlternativaDto
                {
                    Letra = ((char)('A' + index)).ToString(),
                    Texto = a.Descricao,
                    Correta = a.Correta
                }).ToList()

        }).ToList();

        var nomeMateria = teste.Materias
             .Select(m => m.Nome)
             .FirstOrDefault() ?? "Sem matéria";

        var documento = new TestePdfDocument(
            titulo: teste.Titulo,
            disciplina: teste.Disciplina.Nome,
            quantidadeQuestoes: teste.QuantidadeQuestoes,
            provaRecuperacao: teste.ProvaRecuperacao,
            materia: nomeMateria,
            questoes: questoes
            );


        var pdfStream = new MemoryStream();
        documento.GeneratePdf(pdfStream);
        pdfStream.Position = 0;

        return File(pdfStream, "application/pdf", $"Teste_{teste.Titulo}.pdf");
    }

    [HttpGet("gerar-gabarito/{id}")]
    public IActionResult GerarGabaritoPdf(Guid id)
    {
        var teste = _repositorioTeste.SelecionarPorId(id);

        if (teste == null)
            return NotFound();

        var questoes = teste.Questoes.Select(q => new TestePdfDocument.QuestaoDto
        {
            Enunciado = q.Enunciado,

            Alternativas = q.Alternativas
              .Select((a, index) => new TestePdfDocument.AlternativaDto
              {
                  Letra = ((char)('A' + index)).ToString(),
                  Texto = a.Descricao,
                  Correta = a.Correta
              }).ToList()

        }).ToList();

        var nomeMateria = teste.Materias
           .Select(m => m.Nome)
           .FirstOrDefault() ?? "Sem matéria";

        var documento = new TestePdfDocument(
            titulo: teste.Titulo,
            disciplina: teste.Disciplina.Nome,
            quantidadeQuestoes: teste.QuantidadeQuestoes,
            provaRecuperacao: teste.ProvaRecuperacao,
            materia: nomeMateria,
            questoes: questoes,
            mostrarGabarito: true
        );

        var pdfStream = new MemoryStream();
        documento.GeneratePdf(pdfStream);
        pdfStream.Position = 0;

        return File(pdfStream, "application/pdf", $"Gabarito_{teste.Titulo}.pdf");
    }

}