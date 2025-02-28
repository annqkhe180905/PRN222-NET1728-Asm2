using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;

namespace FUNewsManagementSystem
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Account 
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountServices, AccountServices>();

            //Tag
            services.AddScoped<ITagServices, TagServices>();
            services.AddScoped<ITagRepository, TagRepository>();    

            //Category
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            //NewsArticle
            services.AddScoped<INewsArticleServices,NewsArticleServices>();
            services.AddScoped<INewArticleRepository, NewsArticleRepository>();

        }
    }
}
