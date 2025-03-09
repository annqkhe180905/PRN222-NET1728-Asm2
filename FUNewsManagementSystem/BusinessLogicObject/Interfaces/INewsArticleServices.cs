using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface INewsArticleServices 
    {
        Task<IEnumerable<NewsArticleDTO>> GetAllActiveArticles();
            Task<List<NewsArticleDTO>> GetAllNewsAsync();
            Task<NewsArticleDTO?> GetNewsByIdAsync(string newsId);
            Task<List<NewsArticleDTO>> SearchNewsAsync(string query);
            Task CreateNewsAsync(NewsArticleDTO newsDto);
            Task UpdateNewsAsync(NewsArticleDTO newsDto);
            Task DeleteNewsAsync(string newsId);

    }
}
