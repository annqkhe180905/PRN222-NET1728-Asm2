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

        Task<SystemAccount> GetListPagingAcounts(string searchTerm, int pageIndex, int pageSize);

        Task AddAcount(SystemAccount account);
        Task DeleteAcount(short id);
        Task UpdateAcount(SystemAccount account);
        Task<SystemAccount> GetAcountById(short id);
    }
}
