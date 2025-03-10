using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace FUNewsManagementSystem.Pages
{
    public class ProfileModel : BasePageModel
    {
        private readonly IAccountServices _accountServices;

        [BindProperty]
        public AccountDTO Account { get; set; }

        [BindProperty]
        public string? NewPassword { get; set; }

        [BindProperty]
        public IFormFile ProfileImage { get; set; }

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
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"ModelState Error - Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
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

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(ProfileImage.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                user.ImgUrl = fileName;
            }

            await _accountServices.UpdateAccount(user);

            HttpContext.Session.SetString("Account", JsonConvert.SerializeObject(user));

            return RedirectToPage("/staff/Profile");
        }

    }
}
