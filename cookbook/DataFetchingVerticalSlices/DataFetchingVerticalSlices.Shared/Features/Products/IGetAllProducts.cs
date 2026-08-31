using DataFetchingVerticalSlices.Shared.Common;
using DataFetchingVerticalSlices.Shared.Messaging;

namespace DataFetchingVerticalSlices.Shared.Features.Products;

public static class IGetAllProducts
{
    public sealed record Query : IQuery<PagedList<Response>>;
    public sealed class Response
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
    }
    public interface IHandler : IQueryHandler<Query, PagedList<Response>>;
}