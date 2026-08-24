using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Ecommerce.WebApp.Client.Layout.SearchFilters;

namespace Ecommerce.WebApp.Services;

public sealed class CategoryTreeClient(HttpClient httpClient)
{
    private const string Route = "umbraco/delivery/api/v2/custom/category-tree";

    // System.Text.Json ignores nullability and constructor requiredness by default, so a renamed or
    // null API field would deserialize to null instead of throwing.
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        RespectRequiredConstructorParameters = true,
        RespectNullableAnnotations = true,
    };

    public async Task<CategoryTreeResult> GetAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using HttpResponseMessage response = await httpClient.GetAsync(Route, cancellationToken);

            if (response.StatusCode is HttpStatusCode.NotFound)
            {
                return new CategoryTreeResult.NotFound();
            }

            response.EnsureSuccessStatusCode();

            CategoryTreeItem[] tree = await response.Content
                .ReadFromJsonAsync<CategoryTreeItem[]>(SerializerOptions, cancellationToken)
                ?? throw new JsonException($"'{Route}' returned a null payload.");

            return new CategoryTreeResult.Success([.. tree.Select(item => new Category(
                item.Id,
                item.Name,
                item.Color,
                [.. item.Children.Select(child => new SubCategory(child.Id, child.Name, child.Color))]))]);
        }
        // A non-JSON content type means the CMS is answering with something else entirely, e.g. an error page.
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            return new CategoryTreeResult.ContractDrift(ex);
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            return new CategoryTreeResult.Unreachable(ex);
        }
    }

    // The API exposes an arbitrarily deep tree; the filter bar only renders two levels.
    private sealed record CategoryTreeItem(Guid Id, string Name, string? Color, IReadOnlyList<CategoryTreeItem> Children);
}
