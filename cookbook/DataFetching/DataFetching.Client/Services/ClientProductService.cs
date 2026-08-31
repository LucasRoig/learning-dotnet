using System.Net.Http.Json;
using DataFetching.Shared.Features.Products;

namespace DataFetching.Client.Services;

public class ClientProductService : IProductService
{
    private readonly HttpClient _http;

    public ClientProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ProductDTO>> GetProductsAsync()
    {
        // Call the server API. No database access here!
        var products = await _http.GetFromJsonAsync<List<ProductDTO>>("/api/products");
        return products ?? new List<ProductDTO>();
    }
}