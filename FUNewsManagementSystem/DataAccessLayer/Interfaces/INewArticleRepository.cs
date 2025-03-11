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
        Task<NewsArticle?> GetByIdAsync(string newsArticleId);
        Task AddAsync(NewsArticle newsArticle);
        Task UpdateAsync(NewsArticle newsArticle);
        Task DeleteAsync(string id);
        Task<IEnumerable<NewsArticle>> SearchAsync(string keyword, short? categoryId, int[]? tagIds);
        Task<IEnumerable<NewsArticle>> GetNewsHistoryByStaffIdAsync(short staffId);
        Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> CountAsync();
        Task<(string AccountName, int Count)> GetTopAccountWithMostNewsAsync();
    }
}
