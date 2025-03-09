using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class NewsArticleServices : INewsArticleServices
    {
        private readonly INewArticleRepository _newsRepository;
        private readonly IMapper _mapper;

        public NewsArticleServices(INewArticleRepository newsArticleRepository, IMapper mapper)
        {
            _newsRepository = newsArticleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllActiveArticles()
        {
            var news = await _newsRepository.GetAllActiveArticles();
            return _mapper.Map<List<NewsArticleDTO>>(news);
        }

        public async Task<List<NewsArticleDTO>> GetAllNewsAsync()
        {
            var newsArticles = await _newsRepository.GetAllNewsAsync();
            return _mapper.Map<List<NewsArticleDTO>>(newsArticles);
        }

        public async Task<NewsArticleDTO?> GetNewsByIdAsync(string newsId)
        {
            var newsArticle = await _newsRepository.GetByIdAsync(newsId);
            return newsArticle == null ? null : _mapper.Map<NewsArticleDTO>(newsArticle);
        }

        public async Task CreateNewsAsync(NewsArticleDTO newsArticleDto)
        {
            var newsArticle = _mapper.Map<NewsArticle>(newsArticleDto);
            newsArticle.NewsArticleId = Guid.NewGuid().ToString(); // Tạo ID mới cho bài viết
            newsArticle.CreatedDate = DateTime.Now; // Gán thời gian tạo nếu cần
            await _newsRepository.AddAsync(newsArticle);
        }

        public async Task UpdateNewsAsync(NewsArticleDTO newsArticleDto)
        {
            var newsArticle = await _newsRepository.GetByIdAsync(newsArticleDto.NewsArticleId);
            if (newsArticle == null) return;

            _mapper.Map(newsArticleDto, newsArticle); // Cập nhật giá trị từ DTO vào entity
            newsArticle.ModifiedDate = DateTime.Now; // Gán lại thời gian cập nhật nếu cần
            await _newsRepository.UpdateAsync(newsArticle);
        }

        public async Task DeleteNewsAsync(string newsId)
        {
            await _newsRepository.DeleteAsync(newsId);
        }

        public async Task<IEnumerable<NewsArticleDTO>> SearchNewsArticlesAsync(string keyword, short? categoryId, int[]? tagIds)
        {
            var newsArticles = await _newsRepository.SearchAsync(keyword, categoryId, tagIds);
            return newsArticles.Select(MapToDTO);
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetNewsHistoryByStaffIdAsync(short staffId)
        {
            var newsArticles = await _newsRepository.GetNewsHistoryByStaffIdAsync(staffId);
            return newsArticles.Select(MapToDTO);
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetActiveNewsForLecturersAsync()
        {
            var newsArticles = await _newsRepository.GetActiveNewsForLecturersAsync();
            return newsArticles.Select(MapToDTO);
        }

        private NewsArticleDTO MapToDTO(NewsArticle newsArticle)
        {
            return new NewsArticleDTO
            {
                NewsArticleId = newsArticle.NewsArticleId,
                NewsTitle = newsArticle.NewsTitle,
                Headline = newsArticle.Headline,
                CreatedDate = newsArticle.CreatedDate,
                NewsContent = newsArticle.NewsContent,
                NewsSource = newsArticle.NewsSource,
                CategoryId = newsArticle.CategoryId,
                CategoryName = newsArticle.Category?.CategoryName,
                NewsStatus = newsArticle.NewsStatus,
                CreatedById = newsArticle.CreatedById,
                CreatedBy = newsArticle.CreatedBy?.AccountName,
                UpdatedById = newsArticle.UpdatedById,
                ModifiedDate = newsArticle.ModifiedDate,
                ImgUrl = newsArticle.ImgUrl,
                TagIds = newsArticle.Tags?.Select(t => t.TagId).ToList()
            };
        }

        public async Task<List<NewsArticleDTO>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var list = await _newsRepository.GetNewsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<List<NewsArticleDTO>>(list);
        }

        public async Task<int> CountAsync()
        {
            return await _newsRepository.CountAsync();
        }

        public async Task<(string AccountName, int Count)> GetTopAccountWithMostNewsAsync()
        {
            return await _newsRepository.GetTopAccountWithMostNewsAsync();
        }
    }
}
