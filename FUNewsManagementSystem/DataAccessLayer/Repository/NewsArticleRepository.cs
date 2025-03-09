using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
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
                .Include(n => n.Tags)
                .ToListAsync();
        }

        public async Task<NewsArticle?> GetNewsByIdAsync(string newsId)
        {
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

        public async Task CreateNewsAsync(NewsArticle news)
        {
            _context.NewsArticles.Add(news);
            await _context.SaveChangesAsync();
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
    }
}
