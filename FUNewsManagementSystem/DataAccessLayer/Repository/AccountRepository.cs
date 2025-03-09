using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FunewsManagementContext _context;

        public AccountRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.SystemAccounts.AsNoTracking().CountAsync();
        }

        public async Task<List<(string AccountName, int Count)>> GetListTopAccountCreatedNewsAsync()
        {
            var topAccounts = await _context.NewsArticles
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedBy != null)
                .GroupBy(n => n.CreatedBy)
                .Select(g => new { Account = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Select(x => new { AccountName = x.Account.AccountName ?? "Không có tên", Count = x.Count })
                .ToListAsync();

            return topAccounts.Select(x => (x.AccountName, x.Count)).ToList();
        }

        public async Task<SystemAccount> Login(string email)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(x => x.AccountEmail == email && x.Status == true);
        }
    }
}
