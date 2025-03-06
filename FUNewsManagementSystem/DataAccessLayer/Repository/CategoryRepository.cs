using DataAccessLayer.DAOs;
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
        private readonly CategoryDAO categoryDAO;
        public CategoryRepository()
        {
            categoryDAO = new CategoryDAO();
        }

        public async Task<int> Create(Category category)
        {
            bool existCate = await categoryDAO.FindAsync(x => x.CategoryName == category.CategoryName);
            if (existCate)
            {
                return -1;
            }
            return await categoryDAO.Create(category);
        }

        public async Task<bool> Delete(int id)
        {
            bool ishaveNews = await categoryDAO.FindAsync(c => c.CategoryId == id && c.NewsArticles.Count > 0);
            if (ishaveNews)
            {
                return false;
            }
            return await categoryDAO.Delete(id);
        }

        public async Task<List<Category>> GetAllCategory()
        {
            return await categoryDAO.GetAllCategory();
        }

        public async Task<bool> UpdateAsync(Category updateCategory)
        {
            return await categoryDAO.UpdateAsync(updateCategory);
        }
    }
}
