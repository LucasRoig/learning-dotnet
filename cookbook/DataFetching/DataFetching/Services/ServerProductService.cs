namespace DataFetching.Services;

using DataFetching.Shared.Features.Products;

public class ServerProductService : IProductService
{
    public async Task<List<ProductDTO>> GetProductsAsync()
    {
        await Task.Delay(1000);
        return new List<ProductDTO>
        {
            new ProductDTO(1, "Server Product 1", 10.0m),
            new ProductDTO(2, "Server Product 2", 20.0m),
            new ProductDTO(3, "Server Product 3", 30.0m)
        };
    }
}