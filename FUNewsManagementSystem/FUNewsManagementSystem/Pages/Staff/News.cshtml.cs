using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace FUNewsManagementSystem.Pages.Staff
{
    public class NewsModel : BasePageModel
    {
        private readonly INewsArticleServices _newsService;
        private readonly ITagServices _tagService;
        private readonly ICategoryServices _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public List<NewsArticleDTO> NewsList { get; set; } = new List<NewsArticleDTO>();
        public List<CategoryDTO> Categories { get; set; } = new List<CategoryDTO>();
        public List<TagDTO> Tags { get; set; } = new List<TagDTO>();

        public NewsModel(INewsArticleServices newsService, IWebHostEnvironment webHostEnvironment)
        {
            _newsService = newsService;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public NewsArticleDTO NewsArticle { get; set; } = new();

        [BindProperty(SupportsGet = false, Name = "newsImage")]
        public IFormFile? NewsImage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Tags = (await _tagService.GetAllTagsAsync()).ToList();
            Categories = (await _categoryService.GetCategories()).ToList();
            NewsList = await _newsService.SearchNewsAsync(SearchQuery);
            return Page();
        }


        public async Task<IActionResult> OnGetGetNewsAsync(string id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return new JsonResult(news);
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (NewsArticle == null)
            {
                return BadRequest("Invalid news data.");
            }
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
            await _newsService.CreateNewsAsync(NewsArticle);
            return new JsonResult(NewsArticle);
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (NewsArticle == null || string.IsNullOrEmpty(NewsArticle.NewsArticleId))
            {
                return BadRequest("Invalid news data.");
            }
            var existingArticle = await _newsService.GetNewsByIdAsync(NewsArticle.NewsArticleId);
            if (existingArticle == null)
            {
                return NotFound("News article not found.");
            }
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
            return new JsonResult(NewsArticle);
        }

        public async Task<IActionResult> OnPostDeleteAsync([FromBody] string newsId)
        {
            if (string.IsNullOrEmpty(newsId))
            {
                return BadRequest("Invalid ID.");
            }
            await _newsService.DeleteNewsAsync(newsId);
            return new JsonResult(new { message = "News article deleted successfully." });
        }
    }
}
