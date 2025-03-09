using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IAccountServices
    {
        Task<List<AccountDTO>> GetSystemAccounts();

        Task<AccountDTO> GetListPagingAcounts(string searchTerm, int pageIndex, int pageSize);

        Task AddAcount(AccountDTO account);
        Task DeleteAcount(short id);
        Task UpdateAcount(AccountDTO account);
        Task<AccountDTO> GetAcountById(short id);
    }
}
