using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsApp.Foundation.Interfaces;
using NewsApp.WebApp.ViewModels;
using NewsApp.WebApp.ViewModels.News;
using NewsApp.WebApp.ViewModels.Pagination;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.WebApp.Controllers
{
    [Authorize]
    public class HomePageController : Controller
    {
        private const int NewsPageSize = 3;

        private readonly INewsManagementService _newsManagementService;


        public HomePageController(INewsManagementService newsManagementService)
        {
            _newsManagementService = newsManagementService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(string filter, int page = 1)
        {
            var newsPage = await _newsManagementService.GetNewsPageAsync((page - 1) * NewsPageSize, NewsPageSize, filter);

            var newsViewModels = newsPage.Items.Select(n => new NewsManagementViewModel
            {
                Id = n.Id,
                Title = n.Title,
                Subtitle = n.Subtitle,
                Text = n.Text
            }).ToList();

            var pageViewModel = new ItemsPageViewModel<NewsManagementViewModel>(newsViewModels, newsPage.Count, page, NewsPageSize);
            var showNewsViewModel = new ItemsSearchViewModel<NewsManagementViewModel>(pageViewModel, filter);

            return View(showNewsViewModel);
        }
    }
}