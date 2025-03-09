using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;


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

        public Task CreateAccountAsync(AccountDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAccountAsync(short id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDTO> DisableAccount(short id)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDTO> GetAccountByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<AccountDTO> GetAccountByIdAsync(short accountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccountDTO>> GetAllAccountsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasRelatedEntitiesAsync(short id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailUniqueAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountDTO> Login(string email, string password)
        {
            if (email.IsNullOrEmpty() || password.IsNullOrEmpty()) return null;
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
            if (user == null) return null;

            if (user.AccountPassword != password) return null;
            return _mapper.Map<AccountDTO>(user);
        }

        public IEnumerable<AccountDTO> SearchAccounts(string name, int? role)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAccountAsync(AccountDTO dto)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProfile(AccountDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
