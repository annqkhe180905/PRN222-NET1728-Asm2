using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategory();
        Task<int> Create(Category category);
        Task<bool> Delete(int id);
        Task<bool> UpdateAsync(Category updateCategory);
    }
}
