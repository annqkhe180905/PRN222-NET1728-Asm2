using BusinessLogicLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace FUNewsManagementSystem.Filters
{
    public class PageAuthFilter : IPageFilter
    {
        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var path = httpContext.Request.Path.ToString().ToLower();
            var accountJson = httpContext.Session.GetString("Account");
            var user = accountJson != null ? JsonConvert.DeserializeObject<AccountDTO>(accountJson) : null;

            if(path == "/login" || path=="/home")
            {
                return;
            }
/*
            if (user == null)
            {
                context.Result = new RedirectToPageResult("/login");
                return;
            }

            if (path.StartsWith("/admin") && user.AccountRole != 0)
            {
                context.Result = new RedirectToPageResult("/login");
                return;
            }
            else if (path.StartsWith("/staff") && user.AccountRole != 1)
            {
                context.Result = new RedirectToPageResult("/login");
                return;
            }*/
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }
    }
}
