using FUNewsManagementSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsManagementSystem.Helpers
{
    public class BasePageModel : PageModel
    {
        protected readonly IHubContext<CrudHub> HubContext;
        protected BasePageModel(IHubContext<CrudHub> hubContext)
        {
            HubContext = hubContext;
        }

        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToPage("/Login");
        }

        protected async Task NotifyCreate<T>(T entity) where T : class
        {
            string entityName = typeof(T).Name;
            await HubContext.Clients.All.SendAsync("EntityCreated", entityName, entity);
        }

        protected async Task NotifyUpdate<T>(T entity) where T : class
        {
            string entityName = typeof(T).Name;
            await HubContext.Clients.All.SendAsync("EntityUpdated", entityName, entity);
        }

        protected async Task NotifyDelete<T>(object id) where T : class
        {
            string entityName = typeof(T).Name;
            await HubContext.Clients.All.SendAsync("EntityDeleted", entityName, id);
        }
    }
}
