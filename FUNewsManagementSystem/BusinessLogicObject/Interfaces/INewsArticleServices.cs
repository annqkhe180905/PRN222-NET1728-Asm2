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
        Task CreateNewsAsync(NewsArticleDTO newsArticleDto);
        Task UpdateNewsAsync(NewsArticleDTO newsArticleDto);
        Task DeleteNewsAsync(string newsId);
        Task<IEnumerable<NewsArticleDTO>> SearchNewsArticlesAsync(string keyword, short? categoryId, int[]? tagIds);
        Task<IEnumerable<NewsArticleDTO>> GetNewsHistoryByStaffIdAsync(short staffId);
        Task<IEnumerable<NewsArticleDTO>> GetActiveNewsForLecturersAsync();
    }
}
