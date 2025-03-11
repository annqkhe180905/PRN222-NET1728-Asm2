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
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Pages.Staff
{
    public class NewsModel : BasePageModel
    {
        private readonly INewsArticleServices _newsArticleServices;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public NewsModel(INewsArticleServices newsArticleServices, ICategoryRepository categoryRepository,
                         ITagRepository tagRepository, IWebHostEnvironment webHostEnvironment, IHubContext<CrudHub> hubContext)
                         : base(hubContext)
        {
            _newsArticleServices = newsArticleServices;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<NewsArticleDTO> NewsArticles { get; set; } = new();
        [BindProperty]
        public NewsArticleDTO NewsArticle { get; set; } = new();
        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> TagList { get; set; } = new();
        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            NewsArticles = (await _newsArticleServices.GetAllNewsAsync()).ToList();
            CategoryList = (await _categoryRepository.GetAllCategory())
                .Select(c => new SelectListItem { Value = c.CategoryId.ToString(), Text = c.CategoryName })
                .ToList();

            TagList = (await _tagRepository.GetTags())
                .Select(t => new SelectListItem { Value = t.TagId.ToString(), Text = t.TagName })
                .ToList();

            return Page();
        }

        public async Task<JsonResult> OnGetNewsDetail(string id)
        {
            var news = await _newsArticleServices.GetNewsByIdAsync(id);
            if (news == null) return new JsonResult("News not found") { StatusCode = 404 };
            return new JsonResult(news);
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Failed to update news. Please check your input.";
                return Page();
            }
            var userJson = HttpContext.Session.GetString("Account");

            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "User session expired. Please log in.";
                return Page();
            }

            var user = JsonConvert.DeserializeObject<AccountDTO>(userJson);
            var existingNews = await _newsArticleServices.GetNewsByIdAsync(NewsArticle.NewsArticleId);
            if (existingNews == null)
            {
                TempData["ErrorMessage"] = "News article not found.";
                return Page();
            }

            existingNews.UpdatedById = user.AccountId;
            existingNews.NewsTitle = NewsArticle.NewsTitle;
            existingNews.Headline = NewsArticle.Headline;
            existingNews.NewsSource = NewsArticle.NewsSource;
            existingNews.CategoryId = NewsArticle.CategoryId;
            existingNews.NewsStatus = NewsArticle.NewsStatus;
            existingNews.NewsContent = NewsArticle.NewsContent;
            existingNews.Tags = SelectedTagIds.Select(id => new TagDTO { TagId = id }).ToList();

            if (ImageFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    existingNews.ImgUrl = $"/img/{fileName}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Failed to upload image: " + ex.Message;
                    return Page();
                }
            }

            await _newsArticleServices.UpdateNewsAsync(existingNews);
            await NotifyUpdate(existingNews);
            TempData["SuccessMessage"] = "News updated successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Failed to create news. Please check your input.";
                return Page();
            }

            var userJson = HttpContext.Session.GetString("Account");
            if (string.IsNullOrEmpty(userJson))
            {
                TempData["ErrorMessage"] = "User session expired. Please log in.";
                return Page();
            }

            var user = JsonConvert.DeserializeObject<AccountDTO>(userJson);
            NewsArticle.CreatedById = user?.AccountId;

            if (ImageFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", fileName);

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    NewsArticle.ImgUrl = $"/img/{fileName}";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Failed to upload image: " + ex.Message;
                    return Page();
                }
            }

            NewsArticle.Tags = SelectedTagIds.Select(id => new TagDTO { TagId = id }).ToList();
            await _newsArticleServices.CreateNewsAsync(NewsArticle);
            await NotifyCreate(NewsArticle);
            TempData["SuccessMessage"] = "News created successfully!";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteNewsAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["ErrorMessage"] = "Invalid news ID.";
                return Page();
            }

            var news = await _newsArticleServices.GetNewsByIdAsync(id);
            if (news == null)
            {
                TempData["ErrorMessage"] = "News not found.";
                return Page();
            }

            await _newsArticleServices.DeleteNewsAsync(id);
            await NotifyDelete<NewsArticleDTO>(id); 
            TempData["SuccessMessage"] = "News deleted successfully!";
            return RedirectToPage();
        }
    }
}