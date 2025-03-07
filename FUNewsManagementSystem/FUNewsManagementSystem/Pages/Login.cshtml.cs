using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;

namespace FUNewsManagementSystem.Pages
{
    public class LoginModel : PageModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        private readonly IAccountServices _accountServices;

        public LoginModel(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Account") != null )
            {
                var userJson = HttpContext.Session.GetString("Account");
                var user = JsonConvert.DeserializeObject<AccountDTO>(userJson);

                // Redirect based on user role
                return user.AccountRole switch
                {
                    1 => Redirect("/staff"),
                    2 => Redirect("/home"),
                    0 => Redirect("/admin"),
                    _ => RedirectToPage("/login") 
                };
            }

            return Page(); 
        }

        public async Task<IActionResult> OnPostAsync(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ErrorMessage = "Email and password are required.";
                return Page();
            }

            var account = await _accountServices.Login(email, password);

            if (account == null)
            {
                ErrorMessage = "Invalid credentials.";
                return Page();
            }

            var accountJson = JsonConvert.SerializeObject(account);
            HttpContext.Session.SetString("Account", accountJson);

            switch (account.AccountRole)
            {
                case 1:
                    return Redirect("/staff");
                case 2:
                    return Redirect("/home");
                case 0:
                    return Redirect("/admin");
                default:
                    return Page();
            }

        }
    }
}
