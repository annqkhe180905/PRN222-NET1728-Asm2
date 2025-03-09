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
        Task<int> CountAsync();
        Task<List<(string AccountName, int Count)>> GetListTopAccountCreatedNewsAsync();
        public Task<SystemAccount> Login(string email);
    }
}
