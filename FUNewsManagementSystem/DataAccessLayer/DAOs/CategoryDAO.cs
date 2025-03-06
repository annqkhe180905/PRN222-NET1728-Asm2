using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DAOs
{
    public class CategoryDAO
    {
        private readonly FunewsManagementContext _dbContext;
        public CategoryDAO()
        {
            _dbContext = new FunewsManagementContext();
        }
        public async Task<List<Category>> GetAllCategory()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        public async Task<int> Create(Category category)
        {
            try
            {
                await _dbContext.Categories.AddAsync(category);
                await _dbContext.SaveChangesAsync();
                Console.WriteLine(category.CategoryId);
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

                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
                if (category == null)
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

        public async Task<bool> FindAsync(Expression<Func<Category, bool>> predicate)
        {
            try
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(predicate);
                return category != null ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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
