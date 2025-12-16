using CategoriasMvc.Models;
using CategoriasMvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace CategoriasMvc.Controllers;

public class AccountController : Controller
{
    private readonly IAutenticacao _authService;

    public AccountController(IAutenticacao authService)
    {
        _authService = authService;
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(UsuarioViewModel model)
    {
        // verifica se modelo é valido
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Login Inválido...");

            return View(model);
        }

        // verifica as credenciais do usuário e retorna um valor
        var result = await _authService.AutenticaUsuario(model);

        if (result is null)
        { 
            ModelState.AddModelError(string.Empty, "Login Inválido...");

            return View(model);
        }

        Response.Cookies.Append("X-Access-Token", result.Token, new CookieOptions()
        { 
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
        });

        return Redirect("/");
    }
}
