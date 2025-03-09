using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Pages.Staff
{
    public class NewsModel : BasePageModel
    {
        private readonly INewsArticleServices _newsService;
        private readonly ITagServices _tagService;
        private readonly ICategoryServices _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public List<NewsArticleDTO> NewsList { get; set; } = new();
        public List<CategoryDTO> Categories { get; set; } = new();
        public List<TagDTO> Tags { get; set; } = new();

        public NewsModel(INewsArticleServices newsService, IWebHostEnvironment webHostEnvironment,
                         ITagServices tagService, ICategoryServices categoryService)
        {
            _newsService = newsService;
            _webHostEnvironment = webHostEnvironment;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        [BindProperty]
        public NewsArticleDTO NewsArticle { get; set; } = new();

        [BindProperty]
        public IFormFile? NewsImage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Tags = (await _tagService.GetAllTagsAsync()).ToList();
            Categories = (await _categoryService.GetCategories()).ToList();

            // Lọc bài viết theo tiêu đề nếu có tìm kiếm
            NewsList = await _newsService.GetAllNewsAsync();
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                NewsList = NewsList
                    .Where(n => n.NewsTitle.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetGetNewsAsync(string id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound(new { message = "News article not found." });
            }
            return new JsonResult(news);
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid || NewsArticle == null)
            {
                return BadRequest(new { message = "Invalid news data." });
            }

            // Xử lý upload ảnh nếu có
            if (NewsImage != null && NewsImage.Length > 0)
            {
                // Create uploads directory if it doesn't exist
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(NewsImage.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await NewsImage.CopyToAsync(fileStream);
                }

                NewsArticle.ImgUrl = $"/uploads/{fileName}";
            }

            await _newsService.CreateNewsAsync(NewsArticle);
            return new JsonResult(new { message = "News article created successfully.", news = NewsArticle });
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid || NewsArticle == null || string.IsNullOrEmpty(NewsArticle.NewsArticleId))
            {
                return BadRequest(new { message = "Invalid news data." });
            }

            var existingArticle = await _newsService.GetNewsByIdAsync(NewsArticle.NewsArticleId);
            if (existingArticle == null)
            {
                return NotFound(new { message = "News article not found." });
            }

            // Nếu có ảnh mới, cập nhật; nếu không, giữ ảnh cũ
            if (NewsImage != null && NewsImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(NewsImage.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await NewsImage.CopyToAsync(fileStream);
                }
                NewsArticle.ImgUrl = $"/uploads/{fileName}";
            }
            else
            {
                NewsArticle.ImgUrl = existingArticle.ImgUrl;
            }

            await _newsService.UpdateNewsAsync(NewsArticle);
            return new JsonResult(new { message = "News article updated successfully.", news = NewsArticle });
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] string newsId)
        {
            if (string.IsNullOrEmpty(newsId))
            {
                return BadRequest(new { message = "Invalid ID." });
            }

            var existingArticle = await _newsService.GetNewsByIdAsync(newsId);
            if (existingArticle == null)
            {
                return NotFound(new { message = "News article not found." });
            }

            await _newsService.DeleteNewsAsync(newsId);
            return new JsonResult(new { message = "News article deleted successfully." });
        }
    }
}
