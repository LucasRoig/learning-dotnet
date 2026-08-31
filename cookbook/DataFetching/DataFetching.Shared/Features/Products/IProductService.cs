namespace DataFetching.Shared.Features.Products;

public interface IProductService
{
    Task<List<ProductDTO>> GetProductsAsync();
}

