using System.Net.Http.Json;
using DataFetchingVerticalSlices.Shared.Common;
using DataFetchingVerticalSlices.Shared.Features.Products;

namespace DataFetchingVerticalSlices.Client.Features.Products;

class GetAllProductsClient(HttpClient httpClient) : IGetAllProducts.IHandler
{
    public async Task<Result<PagedList<IGetAllProducts.Response>>> Handle(IGetAllProducts.Query query, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetFromJsonAsync<PagedList<IGetAllProducts.Response>>("/api/products", cancellationToken);
        return response;
    }
}