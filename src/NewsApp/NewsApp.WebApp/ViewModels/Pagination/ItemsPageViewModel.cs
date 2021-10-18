using System;
using System.Collections.Generic;

namespace NewsApp.WebApp.ViewModels.Pagination
{
    public class ItemsPageViewModel<T>
    {
        public IReadOnlyCollection<T> Items { get; }

        public int Count { get; }

        public int PageNumber { get; }

        public int TotalPages { get; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;


        public ItemsPageViewModel(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            Count = count;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }
}