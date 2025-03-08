using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Helpers
{
    public class BasePageModel : PageModel
    {
        public IActionResult OnPostLogout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToPage("/Login");
        }
    }
}
