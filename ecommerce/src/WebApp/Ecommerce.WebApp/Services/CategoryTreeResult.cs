using Ecommerce.WebApp.Client.Layout.SearchFilters;

namespace Ecommerce.WebApp.Services;

public abstract record CategoryTreeResult
{
    // Private constructor closes the hierarchy: no other type can derive from it.
    private CategoryTreeResult() { }

    public sealed record Success(Category[] Categories) : CategoryTreeResult;

    public sealed record Unreachable(Exception Exception) : CategoryTreeResult;

    // The CMS answered, but no '/categories' root is published.
    public sealed record NotFound : CategoryTreeResult;

    // The response no longer matches the expected shape: a bug on one side, not an outage.
    public sealed record ContractDrift(Exception Exception) : CategoryTreeResult;
}
