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
using System.IO;

namespace NewsApp.WebApp.Controllers
{
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


        [Authorize(Roles = RoleNames.Admin)]
        public IActionResult Index()
        {
            var newsViewModel = new NewsManagementViewModel();

            return View(newsViewModel);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsManagementViewModel model)
        {
            var news = GetNews(model);
            news.CreatorId = User.GetId();

            if (model.Image != null)
            {
                var isFormatCorrect = _newsManagementService.ChechImageFormat(model.Image.ContentType);
                if (!isFormatCorrect)
                {
                    ModelState.AddModelError(nameof(NewsManagementViewModel.Image), _localizer["WrongFormat"]);
                }
            }

            if (ModelState.IsValid)
            {
                var createNewsResult = await _newsManagementService.CreateNewsAsync(news);

                if (createNewsResult.IsSuccessful)
                {
                    return RedirectToAction("Index", "HomePage");
                }

                foreach (var error in createNewsResult.Errors)
                {
                    var (key, message) = GetErrorMessage(error);
                    ModelState.AddModelError(key, message);
                }
            }

            return View("Index", model);
        }

        [Authorize(Roles = RoleNames.Admin)]
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

        [Authorize]
        public async Task<IActionResult> ShowOne(int id)
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
                ImageData = news.ImageData
            };

            return View(newsManagementViewModel);
        }

        [Authorize(Roles = RoleNames.Admin)]
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
                ImageData = news.ImageData
            };

            return View(newsManagementViewModel);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsManagementViewModel model)
        {
            var news = await _newsManagementService.GetNewsByIdAsync(model.Id);
            if (news == null)
            {
                return NotFound();
            }

            if (model.Image != null)
            {
                var isFormatCorrect = _newsManagementService.ChechImageFormat(model.Image.ContentType);
                if (!isFormatCorrect)
                {
                    ModelState.AddModelError(nameof(NewsManagementViewModel.Image), _localizer["WrongFormat"]);
                }
            }

            if (ModelState.IsValid)
            {
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
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Admin)]
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

            if (model.Image != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.Image.Length);
                }
                news.ImageData = imageData;
            }

            return news;
        }

        private (string modelPropertyName, string message) GetErrorMessage(NewsManagementError value)
        {
            return value switch
            {
                NewsManagementError.EmptyNewsTitle => (nameof(NewsManagementViewModel.Title), _localizer["EmptyNewsTitle"]),
                NewsManagementError.EmptyNewsSubtitle => (nameof(NewsManagementViewModel.Subtitle), _localizer["EmptyNewsSubtitle"]),
                NewsManagementError.EmptyNewsText => (nameof(NewsManagementViewModel.Text), _localizer["EmptyNewsText"]),
                NewsManagementError.NewsTitleTooLong => (nameof(NewsManagementViewModel.Title), _localizer["NewsTitleTooLong"]),
                NewsManagementError.NewsSubtitleTooLong => (nameof(NewsManagementViewModel.Subtitle), _localizer["NewsSubtitleTooLong"]),
                NewsManagementError.NewsTextTooLong => (nameof(NewsManagementViewModel.Text), _localizer["NewsTextTooLong"]),
                _ => ("", "Unknown error")
            };
        }
    }
}