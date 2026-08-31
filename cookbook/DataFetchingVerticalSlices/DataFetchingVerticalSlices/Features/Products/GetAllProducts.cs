

using DataFetchingVerticalSlices.Shared.Messaging;
using DataFetchingVerticalSlices.Shared.Common;
using DataFetchingVerticalSlices.Common;
using DataFetchingVerticalSlices.Endpoints;
using DataFetchingVerticalSlices.Shared.Features.Products;

namespace DataFetchingVerticalSlices.Features.Products;

static class GetAllProducts
{
    internal sealed class Handler() : IGetAllProducts.IHandler
    {
        public async Task<Result<PagedList<IGetAllProducts.Response>>> Handle(IGetAllProducts.Query query, CancellationToken cancellationToken)
        {
            var products = new List<IGetAllProducts.Response>
            {
                new() { Id = Guid.NewGuid(), Name = "Server Product 1", Price = 10.0m },
                new() { Id = Guid.NewGuid(), Name = "Server Product 2", Price = 20.0m },
                new() { Id = Guid.NewGuid(), Name = "Server Product 3", Price = 30.0m }
            };
            return SimplePagedList.Create(products);
        }
    }

    sealed class Endpoint : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async (IQueryHandler<IGetAllProducts.Query, PagedList<IGetAllProducts.Response>> handler, CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new IGetAllProducts.Query(), cancellationToken);
                return result.Match(Results.Ok, CustomResults.Problem);
            });
        }
    }
}