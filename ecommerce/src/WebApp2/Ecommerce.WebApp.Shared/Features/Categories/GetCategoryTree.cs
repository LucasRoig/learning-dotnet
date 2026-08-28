using Ecommerce.WebApp.Shared.Common;
using Ecommerce.WebApp.Shared.Messaging;

namespace Ecommerce.WebApp.Shared.Features.Categories;

public record SubCategoryDto(Guid Id, string Name, string? Color);
public static class IGetCategoryTree
{
    public sealed record Query : IQuery<PagedList<Response>>;
    public sealed class Response
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Color { get; set; }

        public required SubCategoryDto[] Children { get; set; }
    }
    
    public interface IHandler : IQueryHandler<Query, PagedList<Response>>;
}