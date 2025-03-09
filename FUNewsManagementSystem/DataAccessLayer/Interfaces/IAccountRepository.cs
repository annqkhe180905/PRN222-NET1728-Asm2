using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<SystemAccount>> GetSystemAccounts();

        Task<SystemAccount> GetListPagingAccounts(string searchTerm, int pageIndex, int pageSize);

        Task AddAccount(SystemAccount account);
        Task DeleteAccount(short id);
        Task UpdateAccount(SystemAccount account);
        Task<SystemAccount> GetAccountById(short id);

        Task<int> CountAsync();
        Task<List<(string AccountName, int Count)>> GetListTopAccountCreatedNewsAsync();
        public Task<SystemAccount> Login(string email);
    }
}
