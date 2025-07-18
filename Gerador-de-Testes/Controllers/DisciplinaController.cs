using Gerador_de_Testes.Dominio.Compartilhado;
using Gerador_de_Testes.Dominio.ModuloDisciplinas;
using Gerador_de_Testes.Infraestrutura.Orm.ModuloDisciplinas;
using Gerador_de_Testes.WebApp.ActionFilters;
using Gerador_de_Testes.WebApp.Extensions;
using Gerador_de_Testes.WebApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_Testes.WebApp.Controllers
{
    [Route("disciplina")]
    [ValidarModelo]
    public class DisciplinaController : Controller
    {

        private readonly ContextoDados contextoDados;
        private readonly IRepositorioDisciplina repositorioDisciplina;
        private readonly IRepositorioQuestao repositorioQuestao;


        public DisciplinaController(ContextoDados contextoDados, IRepositorioDisciplina repositorioDisciplina, IRepositorioQuestao repositorioQuestao)
        {
            this.contextoDados = contextoDados;
            this.repositorioDisciplina = repositorioDisciplina;
            this.repositorioQuestao = repositorioQuestao;
        }

        public IActionResult Index()
        {
            ViewBag.Titulo = "Disciplinas";
            ViewBag.Header = "Visualizando Questões";
            var disciplinas = repositorioDisciplina.ObterTodos();
            var visualizarVM = new VisualizarDisciplinaViewModel(registros);
            return View(disciplinas);
        }
        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            ViewBag.Titulo = "Disciplinas | Cadastrar";
            ViewBag.Header = "Cadastro de Disciplina";
            var cadastrarVm = new CadastrarDisciplinaViewModel();
            return View(new CadastrarDisciplinaViewModel());
        }
        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(CadastrarDisciplinaViewModel cadastrarVm)
        {

            ViewBag.Titulo = "Disciplinas | Cadastrar";
            ViewBag.Header = "Cadastro de Disciplina";
            var registros = repositorioDisciplina.ObterTodos();

            foreach (var item in registros)
            {
                if (item.Nome.Equals(cadastrarVm.Nome))
                {
                    ModelState.AddModelError("Nome", "Já existe uma disciplina com esse nome.");
                    return View(cadastrarVm);
                }

            }
            var entidade = cadastrarVm.ParaEntidade();
            repositorioDisciplina.Cadastrar(entidade);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("editar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Guid id)
        {
            ViewBag.Titulo = "Disciplinas | Editar";
            ViewBag.Header = "Edição de Disciplina";
            var registros = repositorioDisciplina.ObterPorId(id);

            var editarVm = new EditarDisciplinaViewModel(registros.Id, registros.Nome);
            return View(editarVm);
        }
        [HttpPost("editar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Guid id, EditarDisciplinaViewModel editarVm)
        {
            ViewBag.Titulo = "Disciplinas | Editar";
            ViewBag.Header = "Edição de Disciplina";
            var registros = repositorioDisciplina.ObterTodos();

            foreach (var item in registros)
            {
                if (item.Nome.Equals(editarVm.Nome) && item.Id != id)
                {
                    ModelState.AddModelError("CadastroUnico", "Já existe uma disciplina com esse nome.");
                    return View(editarVm);
                }
            }
            var entidade = editarVm.ParaEntidade();
            
            repositorioDisciplina.Editar(entidade);

            return RedirectToAction(nameof(Index));
        }
        [HttpGet("excluir/{id:guid}")]
        public IActionResult Excluir(Guid id)
        {
            ViewBag.Titulo = "Disciplinas | Excluir";
            ViewBag.Header = "Exclusão de Disciplina";
            var disciplina = repositorioDisciplina.ObterPorId(id);
            if (disciplina == null)
                return NotFound();
            var excluirVm = new ExcluirDisciplinaViewModel(disciplina.Id, disciplina.Nome);
            return View(excluirVm);
        }
        [HttpPost("excluir/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirConfirmado(Guid id, ExcluirDisciplinaViewModel excluirVm)
        {
            ViewBag.Titulo = "Disciplinas | Excluir";
            ViewBag.Header = "Exclusão de Disciplina";
            var disciplina = repositorioDisciplina.ObterPorId(id);
            if (disciplina == null)
                return NotFound();
            var questoes = repositorioQuestao.ObterTodos().Where(q => q.DisciplinaId == id).ToList();
            if (questoes.Any())
            {
                ModelState.AddModelError("Exclusao", "Não é possível excluir uma disciplina que possui questões associadas.");
                return View(excluirVm);
            }
            repositorioDisciplina.Excluir(disciplina);
            return RedirectToAction(nameof(Index));
        }
}


