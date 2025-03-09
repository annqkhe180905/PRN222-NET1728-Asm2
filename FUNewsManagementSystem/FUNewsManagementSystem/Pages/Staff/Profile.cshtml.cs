using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FUNewsManagementSystem.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly IAccountServices _accountServices;

        [BindProperty]
        public AccountDTO Account { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }

        public ProfileModel(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        public IActionResult OnGet()
        {
            var userJson = HttpContext.Session.GetString("Account");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToPage("/Login");
            }

            Account = JsonConvert.DeserializeObject<AccountDTO>(userJson);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userJson = HttpContext.Session.GetString("Account");
            if (string.IsNullOrEmpty(userJson))
            {
                return RedirectToPage("/Login");
            }

            var user = JsonConvert.DeserializeObject<AccountDTO>(userJson);

            user.AccountName = Account.AccountName;
            user.AccountEmail = Account.AccountEmail;

            if (!string.IsNullOrEmpty(NewPassword))
            {
                user.AccountPassword = NewPassword;
            }


           await _accountServices.UpdateAccount(user);

          
                HttpContext.Session.SetString("Account", JsonConvert.SerializeObject(user));

            return RedirectToPage("/Profile");
        }
    }
}
