using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class NewsArticleRepository : INewArticleRepository
    {
        private readonly FunewsManagementContext _context;

        public NewsArticleRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllActiveArticles()
        {
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.NewsStatus == true)
                .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetAllNewsAsync()
        {
            var articles = await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .ToListAsync();

            return articles.Select(n => new NewsArticle
            {
                NewsArticleId = n.NewsArticleId,
                NewsTitle = n.NewsTitle,
                Headline = n.Headline,
                Category = new Category { CategoryId = n.Category.CategoryId, CategoryName = n.Category.CategoryName },
                Tags = n.Tags.Select(t => new Tag { TagId = t.TagId, TagName = t.TagName }).ToList()
            }).ToList();
        }


        public async Task<NewsArticle?> GetNewsByIdAsync(string newsId)
        {
            if (!newsId.Trim().All(char.IsDigit)) // Kiểm tra ID có phải là số không
            {
                return null;
            }

            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .FirstOrDefaultAsync(n => n.NewsArticleId == newsId);
        }


        public async Task<List<NewsArticle>> SearchNewsAsync(string query)
        {
            return await _context.NewsArticles
                .Where(n => n.NewsTitle.Contains(query) || n.Headline.Contains(query))
                .Include(n => n.Category)
                .Include(n => n.Tags)
                .ToListAsync();
        }

        public async Task<NewsArticle?> CreateNewsArticleAsync(NewsArticle newArticle)
        {
            newArticle = await GenerateNextNewsArticleIdAsync(newArticle);
            newArticle.CreatedDate = DateTime.UtcNow;

            _context.NewsArticles.Add(newArticle);
            var saved = await _context.SaveChangesAsync() > 0;

            return saved ? newArticle : null;
        }

        public async Task UpdateNewsAsync(NewsArticle news)
        {
            _context.NewsArticles.Update(news);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsAsync(string newsId)
        {
            var news = await _context.NewsArticles.FindAsync(newsId);
            if (news != null)
            {
                _context.NewsArticles.Remove(news);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.NewsArticles.AsNoTracking().Where(x => x.CreatedDate >= startDate && x.CreatedDate <= endDate).ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.NewsArticles.AsNoTracking().CountAsync();
        }

        public async Task<(string AccountName, int Count)> GetTopAccountWithMostNewsAsync()
        {
            var topAccount = await _context.NewsArticles
                .Include(n => n.CreatedBy)
                .GroupBy(g => g.CreatedBy)
                .Select(x => new { Account = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count)
                .FirstOrDefaultAsync();
            return (topAccount == null) ? ("None", 0) : (topAccount.Account.AccountName, topAccount.Count);
        }

        // Generate the next ID by finding the highest numeric ID and incrementing it
        public async Task<NewsArticle> GenerateNextNewsArticleIdAsync(NewsArticle newArticle)
        {
            var maxId = _context.NewsArticles
                .AsEnumerable() // Chuyển dữ liệu sang xử lý phía C#
                .Where(n => n.NewsArticleId.All(char.IsDigit)) // Lọc các ID có dạng số
                .Select(n => int.Parse(n.NewsArticleId))
                .DefaultIfEmpty(0)
                .Max();

            newArticle.NewsArticleId = (maxId + 1).ToString();
            return newArticle;

        }

    }
}
