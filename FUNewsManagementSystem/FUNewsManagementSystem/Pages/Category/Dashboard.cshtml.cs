using BusinessLogicLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static FUNewsManagementSystem.Pages.Category.DashboardModel;

namespace FUNewsManagementSystem.Pages.Category
{
    public class DashboardModel : PageModel
    {
        private readonly ICategoryServices _categoryServices;
        private readonly IAccountServices _accountServices;
        private readonly INewsArticleServices _newsArticleServices;
        private readonly ITagServices _tagServices;

        public int AccountsCount { get; set; }
        public int CategorysCount { get; set; }
        public int NewAriclesCount { get; set; }
        public int TagsCount { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime StartDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime EndDate { get; set; }
        public List<DailyStat> Stats { get; set; }
        public List<DailyStat> NewStats { get; set; }
        public DashboardModel(ICategoryServices categoryServices, IAccountServices accountServices, INewsArticleServices newsArticleServices, ITagServices tagServices)
        {
            _categoryServices = categoryServices;
            _accountServices = accountServices;
            _newsArticleServices = newsArticleServices;
            _tagServices = tagServices;
        }
        public async void OnGet()
        {
            AccountsCount = 10;
            CategorysCount = 10;
            NewAriclesCount = 10;
            TagsCount = 10;

            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(7);
        }

        private List<DailyStat> GenerateSampleStats(DateTime start, DateTime end)
        {
            var stats = new List<DailyStat>();
            for (var date = start; date <= end; date = date.AddDays(1))
            {
                stats.Add(new DailyStat
                {
                    Date = date,
                    Value = new Random().Next(1, 100) // Giá trị ngẫu nhiên
                });
            }
            return stats;
        }
    }

    public class DailyStat
    {
        public DateTime Date { get; set; }
        public int Value { get; set; }
    }
}
