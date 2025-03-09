﻿using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            var news = await _newsRepository.GetAllNewsAsync();
            return _mapper.Map<List<NewsArticleDTO>>(news);
        }

        public async Task<NewsArticleDTO?> GetNewsByIdAsync(string newsId)
        {
            var news = await _newsRepository.GetNewsByIdAsync(newsId);
            return news != null ? _mapper.Map<NewsArticleDTO>(news) : null;
        }

        public async Task<List<NewsArticleDTO>> SearchNewsAsync(string query)
        {
            var news = await _newsRepository.SearchNewsAsync(query);
            return _mapper.Map<List<NewsArticleDTO>>(news);
        }

        public async Task CreateNewsAsync(NewsArticleDTO newsDto)
        {
            var news = _mapper.Map<NewsArticle>(newsDto);
            news.CreatedDate = DateTime.UtcNow;                       
            await _newsRepository.CreateNewsArticleAsync(news);
        }

        public async Task UpdateNewsAsync(NewsArticleDTO newsDto)
        {
            var news = _mapper.Map<NewsArticle>(newsDto);
            news.ModifiedDate = DateTime.UtcNow;
            await _newsRepository.UpdateNewsAsync(news);
        }

        public async Task DeleteNewsAsync(string newsId)
        {
            await _newsRepository.DeleteNewsAsync(newsId);
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
