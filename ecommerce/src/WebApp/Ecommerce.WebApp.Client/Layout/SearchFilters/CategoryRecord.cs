namespace Ecommerce.WebApp.Client.Layout.SearchFilters;

public record Category(Guid Id, string Name, string? Color, SubCategory[] Children);
public record SubCategory(Guid Id, string Name, string? Color);