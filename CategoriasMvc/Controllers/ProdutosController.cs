using Microsoft.AspNetCore.Mvc;

namespace CategoriasMvc.Controllers;

public class ProdutosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
