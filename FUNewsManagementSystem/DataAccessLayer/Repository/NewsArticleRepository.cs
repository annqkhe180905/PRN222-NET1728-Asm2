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
            return await _context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.ModifiedDate)
                .Include(n => n.Tags)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetByIdAsync(string newsArticleId)
        {
            return await _context.NewsArticles.Include(n => n.Category)
                                              .Include(n => n.Tags)
                                              .FirstOrDefaultAsync(n => n.NewsArticleId == newsArticleId);
        }

        public async Task AddAsync(NewsArticle newsArticle)
        {
            await _context.NewsArticles.AddAsync(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NewsArticle newsArticle)
        {
            _context.NewsArticles.Update(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var newsArticle = await GetByIdAsync(id);
            _context.NewsArticles.Remove(newsArticle);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NewsArticle>> SearchAsync(string keyword, short? categoryId, int[]? tagIds)
        {
            var query = _context.NewsArticles.Include(n => n.Category)
                                             .Include(n => n.Tags)
                                             .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(n => n.NewsTitle.Contains(keyword) || n.Headline.Contains(keyword));

            if (categoryId.HasValue)
                query = query.Where(n => n.CategoryId == categoryId.Value);

            if (tagIds != null && tagIds.Any())
                query = query.Where(n => n.Tags.Any(t => tagIds.Contains(t.TagId)));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsHistoryByStaffIdAsync(short staffId)
        {
            return await _context.NewsArticles.Where(n => n.CreatedById == staffId).ToListAsync();
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsForLecturersAsync()
        {
            return await _context.NewsArticles.Where(n => n.NewsStatus == true).ToListAsync();
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
    }
}
