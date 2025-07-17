using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_Testes.WebApp.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
