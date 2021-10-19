using System.Linq;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.Extensions;
using NewsApp.Foundation.Interfaces;
using NewsApp.Foundation.NewsServices;
using NewsApp.WebApp.ViewModels;
using NewsApp.WebApp.ViewModels.Pagination;
using NewsApp.WebApp.ViewModels.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace NewsApp.WebApp.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class NewsController : Controller
    {
        private const int NewsPageSize = 3;

        private readonly INewsManagementService _newsManagementService;
        private readonly IStringLocalizer<NewsController> _localizer;


        public NewsController(INewsManagementService newsManagementService, IStringLocalizer<NewsController> localizer)
        {
            _newsManagementService = newsManagementService;
            _localizer = localizer;
        }


        public IActionResult Index()
        {
            var newsViewModel = new NewsManagementViewModel();

            return View(newsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsManagementViewModel model)
        {
            var news = GetNews(model);
            news.CreatorId = User.GetId();
            var createNewsResult = await _newsManagementService.CreateNewsAsync(news);

            if (createNewsResult.IsSuccessful)
            {
                return RedirectToAction("Index");
            }

            foreach (var error in createNewsResult.Errors)
            {
                var (key, message) = GetErrorMessage(error);
                ModelState.AddModelError(key, message);
            }

            return View("Index", model);
        }

        public async Task<IActionResult> Show(string filter, int page = 1)
        {
            var newsPage = await _newsManagementService.GetNewsPageAsync((page - 1) * NewsPageSize, NewsPageSize, filter);

            var newsViewModels = newsPage.Items.Select(n => new NewsViewModel
            {
                Id = n.Id,
                Title = n.Title,
                Creator = n.Creator.Email,
                ChangeDate = n.ChangeDate
            }).ToList();

            var pageViewModel = new ItemsPageViewModel<NewsViewModel>(newsViewModels, newsPage.Count, page, NewsPageSize);
            var showNewsViewModel = new ItemsSearchViewModel<NewsViewModel>(pageViewModel, filter);

            return View(showNewsViewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var news = await _newsManagementService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            var newsManagementViewModel = new NewsManagementViewModel
            {
                Title = news.Title,
                Subtitle = news.Subtitle,
                Text = news.Text,
            };

            return View(newsManagementViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsManagementViewModel model)
        {
            var news = await _newsManagementService.GetNewsByIdAsync(model.Id);
            if (news == null)
            {
                return NotFound();
            }

            var fromNews = GetNews(model);
            var updateNewsResult = await _newsManagementService.UpdateNewsAsync(news, fromNews);

            if (updateNewsResult.IsSuccessful)
            {
                return RedirectToAction("Show");
            }

            foreach (var error in updateNewsResult.Errors)
            {
                var (key, message) = GetErrorMessage(error);
                ModelState.AddModelError(key, message);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _newsManagementService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            await _newsManagementService.DeleteNewsAsync(news);

            return RedirectToAction("Show");
        }


        private News GetNews(NewsManagementViewModel model)
        {
            var news = new News
            {
                Title = model.Title,
                Subtitle = model.Subtitle,
                Text = model.Text,
            };

            return news;
        }

        private (string modelPropertyName, string message) GetErrorMessage(NewsManagementError value)
        {
            return value switch
            {
                NewsManagementError.EmptyNewsTitle => ("", _localizer["EmptyNewsTitle"]),
                NewsManagementError.EmptyNewsSubtitle => ("", _localizer["EmptyNewsSubtitle"]),
                NewsManagementError.EmptyNewsText => ("", _localizer["EmptyNewsText"]),
                NewsManagementError.NewsTitleTooLong => ("", _localizer["NewsTitleTooLong"]),
                NewsManagementError.NewsSubtitleTooLong => ("", _localizer["NewsSubtitleTooLong"]),
                NewsManagementError.NewsTextTooLong => ("", _localizer["NewsTextTooLong"]),
                _ => ("", "Unknown error")
            };
        }
    }
}