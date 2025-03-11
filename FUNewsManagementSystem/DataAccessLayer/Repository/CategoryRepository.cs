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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FunewsManagementContext _dbContext;
        public CategoryRepository(FunewsManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Categories.AsNoTracking().CountAsync();
        }

        public async Task<int> Create(Category category)
        {
            bool existCate = await _dbContext.Categories.AsNoTracking().AnyAsync(c => c.CategoryName == category.CategoryName);
            if (existCate)
            {
                return -1;
            }
            try
            {
                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();
                return category.CategoryId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {

                var category = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == id);
                if (category == null)
                {
                    return false;
                }
                bool ishaveNews = category.NewsArticles.Count > 0;
                if (ishaveNews)
                {
                    return false;
                }
                _dbContext.Categories.Remove(category);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<Category>> GetAllCategory()
        {
            return await _dbContext.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<List<(string CategoryName, int Count)>> GetListTopCategoriesAsync()
        {
            var tops = await _dbContext.Categories
                .Select(x => new { Name = x.CategoryName, x.NewsArticles.Count })
                .OrderByDescending(x => x.Count)
                .ToListAsync();
            return tops.Select(x => (x.Name, x.Count)).ToList();
        }

        public async Task<(string CategoryName, int Count)> GetTopCategoryUsageAsync()
        {
            var topCategoryUse = await _dbContext.Categories
                .Include(c => c.NewsArticles)
                .Select(s => new { Category = s.CategoryName, s.NewsArticles.Count })
                .OrderByDescending(x => x.Count).FirstOrDefaultAsync();
            return topCategoryUse == null ? ("NONE", 0) : (topCategoryUse.Category, topCategoryUse.Count);
        }

        public async Task<Category?> GetCategoryById(short categoryId)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.CategoryId == categoryId); // ✅ No AsNoTracking() here
        }


        public async Task<bool> UpdateAsync(Category updateCategory)
        {
            try
            {
                _dbContext.Categories.Update(updateCategory);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
