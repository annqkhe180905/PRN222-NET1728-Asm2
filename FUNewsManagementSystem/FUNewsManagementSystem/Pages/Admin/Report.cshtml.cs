using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Admin
{
    public class ReportModel : BasePageModel
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IAccountServices _accountServices;
        private readonly INewsArticleServices _newsArticleServices;
        private readonly ITagServices _tagServices;

        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }
        public List<DailyStat> Stats { get; set; }
        public TopStats TopStats { get; set; }
        public int AccountsCount { get; set; }
        public int NewsCount { get; set; }
        public int CategoriesCount { get; set; }
        public int TagsCount { get; set; }
        public List<(string CategoryName, int Count)> TopCategories { get; set; }
        public List<(string AccountName, int Count)> TopAccountCreateNews { get; set; }
        public ReportModel(ICategoryServices categoryServices, IAccountServices accountServices, INewsArticleServices newsArticleServices, ITagServices tagServices)
        {
            _categoryServices = categoryServices;
            _accountServices = accountServices;
            _newsArticleServices = newsArticleServices;
            _tagServices = tagServices;
        }
        public async Task OnGet()
        {
            if (Request.Query.TryGetValue("startDate", out var startDateValue) && DateTime.TryParse(startDateValue, out var parsedStartDate))
            {
                StartDate = parsedStartDate;
            }
            else
            {
                StartDate = DateTime.Now;
            }

            if (Request.Query.TryGetValue("endDate", out var endDateValue) && DateTime.TryParse(endDateValue, out var parsedEndDate))
            {
                EndDate = parsedEndDate;
            }
            else
            {
                EndDate = DateTime.Now.AddDays(7);
            }
            NewsCount = await _newsArticleServices.CountAsync();
            AccountsCount = await _accountServices.CountAsync();
            CategoriesCount = await _categoryServices.CountAsync();
            TagsCount = 10;
            Stats = (await _newsArticleServices.GetNewsByDateRangeAsync(StartDate, EndDate))
                .GroupBy(x => x.CreatedDate)
                .Select(n => new DailyStat
                {
                    Date = (DateTime)n.Key,
                    Value = n.Count()
                })
                .ToList();

            TopCategories = await _categoryServices.GetListTopCategoriesAsync();
            TopAccountCreateNews = await _accountServices.GetListTopAccountCreatedNewsAsync();
            TopStats = new TopStats
            {
                TopAccountNewsCount = await _newsArticleServices.GetTopAccountWithMostNewsAsync(),
                //TopTagUsageCount = await _tagServices.GetTopTagUsageAsync(),
                TopCategoryUsageCount = await _categoryServices.GetTopCategoryUsageAsync()
            };
        }
    }
    public class TopStats
    {
        public (string AccountName, int Count) TopAccountNewsCount { get; set; }
        //public (string TagName, int Count) TopTagUsageCount { get; set; }
        public (string CategoryName, int Count) TopCategoryUsageCount { get; set; }
    }
    public class DailyStat
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
    }
}

