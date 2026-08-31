using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Delivery.Filters;
using Umbraco.Cms.Api.Delivery.Routing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Navigation;

namespace CMS.Api;

public sealed record CategoryTreeItem(Guid Id, string Name, string? Color, IReadOnlyList<CategoryTreeItem> Children);

[ApiController]
[VersionedDeliveryApiRoute("custom/category-tree")]
[ApiVersion("2.0")]
[MapToApi("delivery")] // Umbraco.Cms.Api.Delivery.Configuration.DeliveryApiConfiguration.ApiName is internal
[DeliveryApiAccess]
// Umbraco's TagActionsByGroupNameTransformer overwrites OpenAPI tags with the group name, so [Tags] has no effect.
[ApiExplorerSettings(GroupName = "Category Tree")]
public sealed class CategoryTreeController : ControllerBase
{
    private const string RootRoute = "/categories";

    // Guards against a misconfigured content structure producing an unbounded recursion.
    private const int MaxDepth = 10;

    private readonly IPublishedContentCache _publishedContentCache;
    private readonly IDocumentNavigationQueryService _navigationQueryService;
    private readonly IPublishedContentStatusFilteringService _statusFilteringService;
    private readonly IDocumentUrlService _documentUrlService;

    public CategoryTreeController(
        IPublishedContentCache publishedContentCache,
        IDocumentNavigationQueryService navigationQueryService,
        IDocumentUrlService documentUrlService,
        IPublishedContentStatusFilteringService statusFilteringService)
    {
        _publishedContentCache = publishedContentCache;
        _navigationQueryService = navigationQueryService;
        _statusFilteringService = statusFilteringService;
        _documentUrlService = documentUrlService;
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<CategoryTreeItem>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
    {
        IPublishedContent? root = await GetDefaultRootAsync();

        return root is null
            ? NotFound()
            : Ok(MapChildren(root, 0));
    }

    private async Task<IPublishedContent?> GetDefaultRootAsync()
    {
        Guid? key = _documentUrlService.GetDocumentKeyByRoute(
            RootRoute,
            culture: null,
            documentStartNodeId: null,
            isDraft: false);

        return key.HasValue
            ? await _publishedContentCache.GetByIdAsync(key.Value)
            : null;
    }

    private IReadOnlyList<CategoryTreeItem> MapChildren(IPublishedContent content, int depth)
    {
        if (depth >= MaxDepth)
        {
            return [];
        }

        return content
            .Children(_navigationQueryService, _statusFilteringService)
            .Select(child => new CategoryTreeItem(child.Key, child.Name, child.Value<string>("color"), MapChildren(child, depth + 1)))
            .OrderBy(item => item.Name)
            .ToArray();
    }
}
