using System.Net;
using System.Text.Json;
using Ecommerce.WebApp.Common;
using Ecommerce.WebApp.Endpoints;
using Ecommerce.WebApp.Shared.Common;
using Ecommerce.WebApp.Shared.Features.Categories;

namespace Ecommerce.WebApp.Features.Categories;

public record SubCategoryDto(Guid Id, string Name, string? Color);
public static class GetCategoryTree
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        RespectRequiredConstructorParameters = true,
        RespectNullableAnnotations = true,
    };

    internal sealed class Handler(HttpClient httpClient) : IGetCategoryTree.IHandler
    {
        private const string Route = "umbraco/delivery/api/v2/custom/category-tree";
        public async Task<Result<PagedList<IGetCategoryTree.Response>>> Handle(IGetCategoryTree.Query query, CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await httpClient.GetAsync(Route, cancellationToken);
            response.EnsureSuccessStatusCode();
            IGetCategoryTree.Response[] tree = await response.Content
                .ReadFromJsonAsync<IGetCategoryTree.Response[]>(SerializerOptions, cancellationToken)
                ?? throw new JsonException($"'{Route}' returned a null payload.");
            return SimplePagedList.Create(tree);
        }
    }

    sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/categories/tree", async (IGetCategoryTree.IHandler handler, CancellationToken cancellationToken) =>
            {
                return await handler.Handle(new IGetCategoryTree.Query(), cancellationToken);
            });
        }
    }
}