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
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public NewsArticleServices(INewArticleRepository newsArticleRepository, IMapper mapper, ICategoryRepository categoryRepository, IAccountRepository accountRepository, ITagRepository tagRepository)
        {
            _newsRepository = newsArticleRepository ?? throw new ArgumentNullException(nameof(newsArticleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _categoryRepository = categoryRepository;
            _accountRepository = accountRepository;
            _tagRepository = tagRepository;
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
            if (string.IsNullOrWhiteSpace(newsId)) throw new ArgumentException("Invalid news ID", nameof(newsId));

            var newsArticle = await _newsRepository.GetByIdAsync(newsId);
            return newsArticle == null ? null : _mapper.Map<NewsArticleDTO>(newsArticle);
        }

        public async Task CreateNewsAsync(NewsArticleDTO newsArticleDto)
        {
            if (newsArticleDto == null) throw new ArgumentNullException(nameof(newsArticleDto));
            SystemAccount user = null;


            var newsArticle = _mapper.Map<NewsArticle>(newsArticleDto);
            if (newsArticleDto.CreatedById.HasValue)
            {
                user = await _accountRepository.GetAccountById(newsArticleDto.CreatedById.Value);
                newsArticle.CreatedBy = user;

            }
            if (newsArticleDto.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetCategoryById(newsArticleDto.CategoryId.Value);

                newsArticle.Category = category;

            }
            List<Tag> tags = new List<Tag>();
            if (newsArticleDto.Tags != null && newsArticleDto.Tags.Any())
            {
                var tagIds = newsArticleDto.Tags.Select(t => t.TagId).ToList();
                newsArticle.Tags = await _tagRepository.GetTagsByIdsAsync(tagIds);
            }
            newsArticle.NewsStatus = false;
            newsArticle.CreatedDate = DateTime.UtcNow.AddHours(7);
            newsArticle.ModifiedDate = DateTime.UtcNow.AddHours(7);
            await _newsRepository.AddAsync(newsArticle);
        }

        public async Task UpdateNewsAsync(NewsArticleDTO newsArticleDto)
        {
            if (newsArticleDto == null || string.IsNullOrWhiteSpace(newsArticleDto.NewsArticleId))
                throw new ArgumentException("Invalid news article data", nameof(newsArticleDto));

            var newsArticle = await _newsRepository.GetByIdAsync(newsArticleDto.NewsArticleId);
            if (newsArticle == null)
                throw new KeyNotFoundException("News article not found");

            _mapper.Map(newsArticleDto, newsArticle);

            if (newsArticleDto.CategoryId.HasValue)
            {
                var category = await _categoryRepository.GetCategoryById(newsArticleDto.CategoryId.Value);
                newsArticle.Category = category;
            }
            if (newsArticleDto.Tags != null && newsArticleDto.Tags.Any())
            {
                var tagIds = newsArticleDto.Tags.Select(t => t.TagId).ToList();
                newsArticle.Tags = await _tagRepository.GetTagsByIdsAsync(tagIds);
            }
            newsArticle.ModifiedDate = DateTime.UtcNow.AddHours(7);
            await _newsRepository.UpdateAsync(newsArticle);
        }


        public async Task DeleteNewsAsync(string newsId)
        {
            if (string.IsNullOrWhiteSpace(newsId)) throw new ArgumentException("Invalid news ID", nameof(newsId));

            var newsArticle = await _newsRepository.GetByIdAsync(newsId);
            if (newsArticle == null) throw new KeyNotFoundException("News article not found");

            await _newsRepository.DeleteAsync(newsId);
        }

        public async Task<IEnumerable<NewsArticleDTO>> SearchNewsArticlesAsync(string keyword, short? categoryId, int[]? tagIds)
        {
            var newsArticles = await _newsRepository.SearchAsync(keyword, categoryId, tagIds);
            return _mapper.Map<List<NewsArticleDTO>>(newsArticles);
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetNewsHistoryByStaffIdAsync(short staffId)
        {
            var newsArticles = await _newsRepository.GetNewsHistoryByStaffIdAsync(staffId);
            return _mapper.Map<List<NewsArticleDTO>>(newsArticles);
        }

        public async Task<List<NewsArticleDTO>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate) throw new ArgumentException("Start date cannot be after end date");

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
