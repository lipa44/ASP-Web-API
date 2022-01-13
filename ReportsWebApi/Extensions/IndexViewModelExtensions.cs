using ReportsWebApi.Filters;
using ReportsWebApi.ViewModels;

namespace ReportsWebApi.Extensions;

public static class IndexViewModelExtensions<T>
{
    public static IndexViewModel<T> ToIndexViewModel(ICollection<T> items, PaginationFilter paginationFilter)
    {
        return new IndexViewModel<T>
        {
            PageViewModel = new (items.Count, paginationFilter.Page, paginationFilter.Take),
            Items = items.Skip((paginationFilter.Page - 1) * paginationFilter.Take)
                .Take(paginationFilter.Take).ToList(),
        };
    }
}