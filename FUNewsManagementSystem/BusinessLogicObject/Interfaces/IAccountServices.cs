﻿using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;

using Microsoft.AspNetCore.Http;
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

        Task<AccountDTO> GetListPagingAccounts(string searchTerm, int pageIndex, int pageSize);

        Task AddAccount(AccountDTO account);
        Task DeleteAccount(short id);
        Task UpdateAccount(AccountDTO account);
        Task<AccountDTO> GetAccountById(short id);
        public Task<AccountDTO> Login(string email, string password);
        
    }
}
