using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace FUNewsManagementSystem.Pages.Home
{
    public class IndexModel : BasePageModel
    {
        private readonly INewsArticleServices _newsArticleServices;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;


        public IndexModel(INewsArticleServices newsArticleServices, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _newsArticleServices = newsArticleServices;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public List<NewsArticleDTO> NewsArticles { get; set; } = new();
        [BindProperty]
        public NewsArticleDTO NewsArticle { get; set; } = new();
        public List<SelectListItem> CategoryList { get; set; } = new();
        public List<SelectListItem> TagList { get; set; } = new();
        [BindProperty]
        public List<int> SelectedTagIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            NewsArticles = (await _newsArticleServices.GetAllActiveArticles()).ToList();
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
    }
}
