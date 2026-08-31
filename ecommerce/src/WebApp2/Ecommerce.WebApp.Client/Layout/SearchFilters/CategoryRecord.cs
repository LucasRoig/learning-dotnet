using Ecommerce.WebApp.Shared.Features.Categories;

namespace Ecommerce.WebApp.Client.Layout.SearchFilters;

public record Category(Guid Id, string Name, string? Color, SubCategory[] Children)
{
    public static Category FromGetCategoryTree(IGetCategoryTree.Response dto) => new(
        Id: dto.Id,
        Name: dto.Name,
        Color: dto.Color,
        Children: dto.Children.Select(c => new SubCategory(c.Id, c.Name, c.Color)).ToArray()
    );
};

public record SubCategory(Guid Id, string Name, string? Color);