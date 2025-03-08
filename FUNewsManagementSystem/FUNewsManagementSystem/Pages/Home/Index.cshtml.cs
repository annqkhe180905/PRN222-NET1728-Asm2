using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Home
{
    public class IndexModel : BasePageModel
    {
        private readonly INewsArticleServices _newsArticleServices;

        public IndexModel(INewsArticleServices newsArticleServices)
        {
            _newsArticleServices = newsArticleServices;
        }

        public List<NewsArticleDTO> NewsArticles { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            NewsArticles = (await _newsArticleServices.GetAllActiveArticles()).ToList();
            return Page();
        }
    }
}
