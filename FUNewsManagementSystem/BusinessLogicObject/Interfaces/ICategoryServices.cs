using BusinessLogicLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ICategoryServices
    {
        Task<List<CategoryDTO>> GetCategories();
        Task<int> Create(CategoryDTO categoryDTO);
        Task<bool> Delete(int id);
        Task<bool> UpdateAsync(CategoryDTO newCategory);
    }
}
