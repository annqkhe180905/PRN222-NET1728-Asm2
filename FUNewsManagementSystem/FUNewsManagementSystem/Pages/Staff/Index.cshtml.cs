using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Pages.Staff
{
    public class IndexModel : BasePageModel
    {
        private readonly ICategoryServices categoryServices;

        public List<CategoryDTO> Categories { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 6;
        public int TotalItems { get; set; } = 6;

        [BindProperty]
        public CategoryDTO newCategory { get; set; }

        [BindProperty]
        public int IdDelete { get; set; }

        public IndexModel(ICategoryServices categoryServices, IHubContext<CrudHub> hubContext)
            : base(hubContext)
        {
            this.categoryServices = categoryServices;
        }

        public async Task OnGetAsync(int pageee = 1)
        {
            CurrentPage = pageee;
            var categories = await categoryServices.GetCategories();
            TotalItems = categories.Count;
            Categories = categories.Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        public async Task<IActionResult> OnPostCreate()
        {
            if (newCategory == null || string.IsNullOrEmpty(newCategory.CategoryName) || string.IsNullOrEmpty(newCategory.CategoryDesciption))
            {
                return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
            }
            try
            {
                int id = await categoryServices.Create(newCategory);
                string replyMsg = id != -1 ? "Thêm category thành công" : "Thêm thất bại, có lỗi xảy ra!";

                if (id != -1)
                {
                    await NotifyCreate(newCategory); 
                }

                return new JsonResult(new
                {
                    success = id != -1,
                    message = replyMsg,
                });
            }
            catch (Exception)
            {
                return BadRequest(new { success = false, message = "Lỗi" });
            }
        }

        public async Task<IActionResult> OnGetDelete(string id)
        {
            try
            {
                bool succeed = await categoryServices.Delete(int.Parse(id));
                string replyMsg = succeed ? "Xóa category thành công" : "Xóa thất bại, category này có news!";

                if (succeed)
                {
                    await NotifyDelete<CategoryDTO>(id); 
                }

                return new JsonResult(new { success = succeed, message = replyMsg });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(new { success = false, message = "Lỗi" });
            }
        }

        public async Task<IActionResult> OnPostUpdate()
        {
            try
            {
                bool succeed = await categoryServices.UpdateAsync(newCategory);
                string replyMsg = succeed ? "Update category success" : "Update fail, Category name or description is existed!";

                if (succeed)
                {
                    await NotifyUpdate(newCategory); 
                }

                return new JsonResult(new { success = succeed, message = replyMsg });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(new { success = false, message = "Lỗi" });
            }
        }
    }
}