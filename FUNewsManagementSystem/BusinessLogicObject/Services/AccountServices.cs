using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IAccountRepository _accountRepository;
        private readonly AdminAccount _adminAccount;
        private readonly IMapper _mapper;

        public AccountServices(IAccountRepository accountRepository, IOptions<AdminAccount> adminAccount, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _adminAccount = adminAccount.Value;
            _mapper = mapper;
        }

     public async Task AddAccount(AccountDTO account)
   {
       var newAccount = _mapper.Map<SystemAccount>(account);
       await _accountRepository.AddAccount(newAccount);
   }
        
        public async Task<int> CountAsync()
        {
            return await _accountRepository.CountAsync();
        }

        public async Task DeleteAccount(short id)
        {
            await _accountRepository.DeleteAccount(id);
        }

        public async Task<AccountDTO> GetAccountById(short id)
        {
            var account = await _accountRepository.GetAccountById(id);
            return _mapper.Map<AccountDTO>(account);
        }

        public async Task<List<AccountDTO>> GetSystemAccounts()
        {
            var accounts = await _accountRepository.GetSystemAccounts();
            return _mapper.Map<List<AccountDTO>>(accounts);
        }

        public async Task UpdateAccount(AccountDTO account)
        {
            var updatedAccount = _mapper.Map<SystemAccount>(account);
            await _accountRepository.UpdateAccount(updatedAccount);

        }

        public async Task<List<(string AccountName, int Count)>> GetListTopAccountCreatedNewsAsync()
        {
            return await _accountRepository.GetListTopAccountCreatedNewsAsync();
        }

        public async Task<AccountDTO> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return null;

            if (email == _adminAccount.Email && password == _adminAccount.Password)
            {
                return new AccountDTO
                {
                    AccountName = "Admin",
                    AccountEmail = email,
                    AccountPassword = password,
                    AccountRole = 0,
                };
            }

            var user = await _accountRepository.Login(email);
            if (user == null || user.AccountPassword != password) return null;

            return _mapper.Map<AccountDTO>(user);
        }

        public IEnumerable<AccountDTO> SearchAccounts(string name, int? role)
        {
            throw new NotImplementedException();
        }

      
    }
}
