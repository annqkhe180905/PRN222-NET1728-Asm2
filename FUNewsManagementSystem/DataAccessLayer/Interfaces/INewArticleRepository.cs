using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface INewArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllActiveArticles();
        Task<List<NewsArticle>> GetAllNewsAsync();
        Task<NewsArticle?> GetNewsByIdAsync(string newsId);
        Task<List<NewsArticle>> SearchNewsAsync(string query);
        Task<NewsArticle?> CreateNewsArticleAsync(NewsArticle newArticle);
        Task UpdateNewsAsync(NewsArticle news);
        Task DeleteNewsAsync(string newsId);

        Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> CountAsync();
        Task<(string AccountName, int Count)> GetTopAccountWithMostNewsAsync();
    }
}
