using NewsApp.WebApp.ViewModels.Pagination;

namespace NewsApp.WebApp.ViewModels
{
    public class ItemsSearchViewModel<T>
    {
        public ItemsPageViewModel<T> ItemsPage { get; }

        public string Filter { get; }


        public ItemsSearchViewModel(ItemsPageViewModel<T> itemsPage, string filter)
        {
            ItemsPage = itemsPage;
            Filter = filter;
        }
    }
}