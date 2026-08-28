using Ecommerce.WebApp.Shared.Common;
using Ecommerce.WebApp.Shared.Features.Categories;
using System.Net.Http.Json;

namespace Ecommerce.WebApp.Client.Features.Categories;

public class GetCategoryTreeClient(HttpClient httpClient) : IGetCategoryTree.IHandler
{
    public async Task<Result<PagedList<IGetCategoryTree.Response>>> Handle(IGetCategoryTree.Query query, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetFromJsonAsync<PagedList<IGetCategoryTree.Response>>("/api/categories/tree", cancellationToken);
        return response;
    }
}