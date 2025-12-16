using CategoriasMvc.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CategoriasMvc.Services;

public class ProdutoService : IProdutoService
{
    private readonly IHttpClientFactory _clientFactory;
    private const string apiEndpoint = "/api/1/produtos";
    private readonly JsonSerializerOptions _options;
    private ProdutoViewModel produtoVM;
    private IEnumerable<ProdutoViewModel> produtosVM;

    public ProdutoService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true }; // para ignorar maiúsculas e minúsculas
    }

    public Task<IEnumerable<ProdutoViewModel>> GetProdutos(string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthorization(token, client);


    }

    public Task<ProdutoViewModel> GetProdutosPorId(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthorization(token, client);
    }

    public Task<ProdutoViewModel> CriaProduto(ProdutoViewModel produtoVM, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthorization(token, client);
    }

    public Task<bool> AtualizaProduto(int id, ProdutoViewModel produtoVM, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthorization(token, client);
    }

    public Task<bool> DeletaProduto(int id, string token)
    {
        var client = _clientFactory.CreateClient("ProdutosApi");

        PutTokenInHeaderAuthorization(token, client);
    }

    // métodos auxiliares
    private static void PutTokenInHeaderAuthorization(string token, HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

}
