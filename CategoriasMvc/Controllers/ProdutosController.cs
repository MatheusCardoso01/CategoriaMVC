using CategoriasMvc.Models;
using CategoriasMvc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CategoriasMvc.Controllers;

public class ProdutosController : Controller
{
    private readonly IProdutoService _produtoService;
    private readonly ICategoriaService _categoriaService;
    private string token;

    public ProdutosController(IProdutoService produtoService, ICategoriaService categoriaService)
    {
        _produtoService = produtoService;
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoViewModel>>> Index()
    {
        // extrai o token do cookie
        var result = await _produtoService.GetProdutos(ObterTokenJwt());

        if (result is null)
            return View("Error");

        return View(result);
    }

    [HttpGet]
    public async Task<IActionResult> CriarNovoProduto()
    {
        var categorias = await CarregaCategorias();

        if (categorias is null)
            return View("Error");

        ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "Nome");

        return View();
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoViewModel>> CriarNovoProduto(ProdutoViewModel produtoVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _produtoService.CriaProduto(produtoVM, ObterTokenJwt());

            if (result is not null)
                return RedirectToAction(nameof(Index));
        }

        ViewBag.Erro = "Erro ao criar Produto";

        return View(produtoVM);
    }

    [HttpGet]
    public async Task<ActionResult<ProdutoViewModel>> AtualizarProduto(int id)
    {
        var produto = await _produtoService.GetProdutosPorId(id, ObterTokenJwt());

        if (produto is null)
            return View("Error");

        var categorias = await CarregaCategorias();

        if (categorias is null)
            return View("Error");

        ViewBag.Categorias = new SelectList(categorias, "CategoriaId", "Nome", produto.CategoriaId);

        return View(produto);

    }

    [HttpPost]
    public async Task<ActionResult<ProdutoViewModel>> AtualizarProduto(ProdutoViewModel produtoVM)
    {
        if (ModelState.IsValid)
        {
            var result = await _produtoService.AtualizaProduto(produtoVM.ProdutoId, produtoVM, ObterTokenJwt());

            if (result)
                return RedirectToAction(nameof(Index));
        }

        ViewBag.Erro = "Erro ao atualizar Produto";

        return View(produtoVM);
    }

    [HttpGet]
    public async Task<ActionResult<ProdutoViewModel>> DeletarProduto(int id)
    {
        var produto = await _produtoService.GetProdutosPorId(id, ObterTokenJwt());

        if (produto is null)
            return RedirectToAction(nameof(Index));

        return View(produto);
    }

    [HttpPost(), ActionName("DeletarProduto")]
    public async Task<ActionResult<ProdutoViewModel>> DeletaConfirmado(int id)
    {
        var result = await _produtoService.DeletaProduto(id, ObterTokenJwt());

        if (result)
            return RedirectToAction(nameof(Index));

        return View("Error");
    }

    [HttpGet]
    public async Task<ActionResult<ProdutoViewModel>> VerDetalhesProduto(int id)
    {
        var produto = await _produtoService.GetProdutosPorId(id, ObterTokenJwt());

        if (produto is null)
            return View("Error");

        var categoria = await _categoriaService.GetCategoriaPorId(produto.CategoriaId);
        ViewBag.CategoriaNome = categoria.Nome;

        return View(produto);
    }

    // métodos auxiliares
    private string ObterTokenJwt()
    {
        if (HttpContext.Request.Cookies.ContainsKey("X-Access-Token"))
            token = HttpContext.Request.Cookies["X-Access-Token"].ToString();

        return token;
    }

    private async Task<IEnumerable<CategoriaViewModel>> CarregaCategorias()
    { 
        var categorias = await _categoriaService.GetCategorias();

        if (categorias is null || !categorias.Any())
            return null;

        return categorias;
    }

}
