using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
        public class CategoryServices : ICategoryServices
        {
            private readonly ICategoryRepository categoryRepository;
            private readonly IMapper mapper;
            public CategoryServices(ICategoryRepository categoryRepository, IMapper mapper)
            {
                this.categoryRepository = categoryRepository;
                this.mapper = mapper;
            }

        public async Task<int> CountAsync()
        {
            return await categoryRepository.CountAsync();
        }

        public async Task<int> Create(CategoryDTO categoryDTO)
        {
            var newCategory = mapper.Map<Category>(categoryDTO);
            return await categoryRepository.Create(newCategory);
        }

            public async Task<bool> Delete(int id)
            {
                return await categoryRepository.Delete(id);
            }

            public async Task<List<CategoryDTO>> GetCategories()
            {
                List<Category> categories = await categoryRepository.GetAllCategory();
                return mapper.Map<List<CategoryDTO>>(categories);
            }

        public async Task<List<(string CategoryName, int Count)>> GetListTopCategoriesAsync()
        {
            return await categoryRepository.GetListTopCategoriesAsync();
        }

        public async Task<(string CategoryName, int Count)> GetTopCategoryUsageAsync()
        {
            return await categoryRepository.GetTopCategoryUsageAsync();
        }

        public async Task<bool> UpdateAsync(CategoryDTO newCategory)
        {
            var updateCategory = mapper.Map<Category>(newCategory);
            return await categoryRepository.UpdateAsync(updateCategory);
        }
    }
}
