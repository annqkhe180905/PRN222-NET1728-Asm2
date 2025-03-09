using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.DAOs;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IAccountRepository _acountRepo;
        private readonly IMapper mapper;
        public AccountServices(IAccountRepository acountRepo, IMapper mapper)
        {
            this._acountRepo = acountRepo;
            this.mapper = mapper;
        }

        public async Task AddAcount(AccountDTO account)
        {
            var newAccount = mapper.Map<SystemAccount>(account);
             await _acountRepo.AddAcount(newAccount);
        }

        public async Task DeleteAcount(short id)
        {
            await _acountRepo.DeleteAcount(id);
        }

        public async Task<AccountDTO> GetAcountById(short id)
        {
            var account = await _acountRepo.GetAcountById(id);
            return mapper.Map<AccountDTO>(account);
        }

        public Task<AccountDTO> GetListPagingAcounts(string searchTerm, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AccountDTO>> GetSystemAccounts()
        {
            List<SystemAccount> accounts = await _acountRepo.GetSystemAccounts();
            return mapper.Map<List<AccountDTO>>(accounts);
        }

        public async Task UpdateAcount(AccountDTO account)
        {
            var newAccount = mapper.Map<SystemAccount>(account);
            await _acountRepo.UpdateAcount(newAccount);
        }
    }
}
