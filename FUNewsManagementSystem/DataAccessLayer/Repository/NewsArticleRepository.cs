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
        private readonly FunewsManagementContext _dbContext;

        public NewsArticleRepository(FunewsManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllActiveArticles()
        {
            return await _dbContext.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.Tags)
                .Where(n => n.NewsStatus == true)
                .ToListAsync();
        }
    }
}
