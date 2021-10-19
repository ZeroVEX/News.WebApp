using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsApp.DomainModel;
using NewsApp.Foundation.Interfaces;
using NewsApp.Repositories.Interfaces;

namespace NewsApp.Foundation.NewsServices
{
    public class NewsManagementService : INewsManagementService
    {
        private readonly INewsUnitOfWork _unitOfWork;


        public NewsManagementService(INewsUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<OperationResult<NewsManagementError>> CreateNewsAsync(News news)
        {
            var errors = GetErrors(news);
            if (errors.Count != 0)
            {
                return OperationResult<NewsManagementError>.Failed(errors);
            }

            news.ChangeDate = DateTime.UtcNow;

            _unitOfWork.GetRepository<News>().Create(news);
            await _unitOfWork.SaveAsync();

            return OperationResult<NewsManagementError>.Success;
        }

        public async Task<OperationResult<NewsManagementError>> UpdateNewsAsync(News news, News fromNews)
        {
            var errors = GetErrors(fromNews);
            if (errors.Count != 0)
            {
                return OperationResult<NewsManagementError>.Failed(errors);
            }

            news.ChangeDate = DateTime.UtcNow;
            news.Title = fromNews.Title;
            news.Subtitle = fromNews.Subtitle;
            news.Text = fromNews.Text;
            if (fromNews.ImageData != null)
            {
                news.ImageData = fromNews.ImageData;
            }

            await _unitOfWork.SaveAsync();

            return OperationResult<NewsManagementError>.Success;
        }

        public async Task DeleteNewsAsync(News news)
        {
            _unitOfWork.NewsRepository.Delete(news);
            await _unitOfWork.SaveAsync();
        }

        public async Task<EntityPage<News>> GetNewsPageAsync(int skip, int take, string filter)
        {
            var news = await _unitOfWork.NewsRepository.GetNewsPageAsync(skip, take, filter);
            var count = await _unitOfWork.NewsRepository.CountNewsAsync(filter);
            var newsPage = new EntityPage<News>(news, count);

            return newsPage;
        }

        public async Task<News> GetNewsByIdAsync(int id)
        {
            return await _unitOfWork.NewsRepository.GetByIdAsync(id);
        }

        public bool ChechImageFormat(string type)
        {
            var isFormatCorrect = type.EndsWith("jpeg") || type.EndsWith("png");

            return isFormatCorrect;
        }

        private static HashSet<NewsManagementError> GetErrors(News news)
        {
            var errors = new HashSet<NewsManagementError>();

            if (string.IsNullOrWhiteSpace(news.Title))
            {
                errors.Add(NewsManagementError.EmptyNewsTitle);
            }
            else if (news.Title.Length > News.TitleMaxLength)
            {
                errors.Add(NewsManagementError.NewsTitleTooLong);
            }

            if (string.IsNullOrWhiteSpace(news.Subtitle))
            {
                errors.Add(NewsManagementError.EmptyNewsSubtitle);
            }
            else if (news.Subtitle.Length > News.SubtitleMaxLength)
            {
                errors.Add(NewsManagementError.NewsSubtitleTooLong);
            }

            if (string.IsNullOrWhiteSpace(news.Text))
            {
                errors.Add(NewsManagementError.EmptyNewsText);
            }
            else if (news.Text.Length > News.TextMaxLength)
            {
                errors.Add(NewsManagementError.NewsTextTooLong);
            }

            return errors;
        }
    }
}