namespace Ecommerce.WebApp.Client.Layout.SearchFilters;

public record Category(string Id, string Name, SubCategory[]? Children);
public record SubCategory(string Id, string Name);