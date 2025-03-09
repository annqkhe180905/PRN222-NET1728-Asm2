using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using FUNewsManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.Pages.Admin
{
    public class IndexModel : BasePageModel
    {
        private readonly IAccountServices _accountService;

        public IndexModel(IAccountServices accountService)
        {
            _accountService = accountService;
        }

        public List<AccountDTO> Accounts { get; set; } = new();

        [BindProperty]
        public AccountDTO InputAccount { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? Role { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Accounts = await _accountService.GetSystemAccounts();
            return Page();
        }


        public List<AccountDTO> SearchResults { get; set; } = new();

        public async Task<IActionResult> OnGetSearch()
        {
            await OnGet();
            if (!string.IsNullOrEmpty(Name))
            {
                Accounts = Accounts.FindAll(a => a.AccountName.Contains(Name));
            }
            if (Role.HasValue)
            {
                Accounts = Accounts.FindAll(a => a.AccountRole == Role);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostCreate()
        {
            try
            {
                await _accountService.AddAccount(InputAccount);
                TempData["SuccessMessage"] = "Account created successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return await OnGet();
            }
        }

        public async Task<IActionResult> OnPostEdit()
        {


            try
            {


                await _accountService.UpdateAccount(InputAccount);
                TempData["SuccessMessage"] = "Account updated successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return await OnGet();
            }

        }

        public async Task<IActionResult> OnPostDelete(short id)
        {
            try
            {
                var account = await _accountService.GetAccountById(id);
                if (account == null)
                {
                    TempData["ErrorMessage"] = "Account not found.";
                    return Page();
                }
                bool newStatus = !account.Status;


                await _accountService.DeleteAccount(id);
                TempData["SuccessMessage"] = newStatus ? "Account enabled successfully." : "Account disabled successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }
    }
}
