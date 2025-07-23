using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_Testes.WebApp.Controllers;
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Title = "Gerador de Testes | Página Inicial";
        return View();
    }

    [HttpGet("error")]
    public IActionResult Error()
    {
        ViewBag.Title = "Gerador de Testes | Ops! Algo deu errado";
        return View();
    }
}