namespace NewsApp.WebApp.ViewModels.Pagination
{
    public class PaginationViewModel
    {
        public int PageNumber { get; }

        public int TotalPages { get; }

        public string Filter { get; }

        public string Action { get; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;


        public PaginationViewModel(int pageNumber, int totalPages, string filter, string action)
        {
            PageNumber = pageNumber;
            TotalPages = totalPages;
            Filter = filter;
            Action = action;
        }
    }
}