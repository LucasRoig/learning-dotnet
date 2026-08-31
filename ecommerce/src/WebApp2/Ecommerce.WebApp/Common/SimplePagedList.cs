using Ecommerce.WebApp.Shared.Common;

namespace Ecommerce.WebApp.Common;

static class SimplePagedList
{

    public static PagedList<T> Create<T>(IEnumerable<T> items)
    {
        return new PagedList<T>
        {
            Items = items.ToArray(),
            TotalCount = items.Count(),
            PageSize = items.Count(),
            Page = 1
        };

    }
}