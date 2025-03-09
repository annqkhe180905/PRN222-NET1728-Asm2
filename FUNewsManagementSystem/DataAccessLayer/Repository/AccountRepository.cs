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

        public async Task<SystemAccount> Login(string email)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(x => x.AccountEmail == email && x.Status== true);
        }
    }
}
