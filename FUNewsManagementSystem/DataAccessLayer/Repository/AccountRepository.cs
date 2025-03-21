﻿using DataAccessLayer.Entities;
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

        public async Task AddAccount(SystemAccount account)
        {
            try
            {
                // Kiểm tra xem email đã tồn tại hay chưa
                bool emailExists = await _context.SystemAccounts.AnyAsync(a => a.AccountEmail == account.AccountEmail);
                if (emailExists)
                {
                    throw new Exception("Email already exists");
                }

                // Tạo ID mới dựa trên số lượng tài khoản hiện có
                account.AccountId = (short)(await _context.SystemAccounts.CountAsync() + 1);
                account.AccountPassword = "@1";
                account.Status = true;

                // Thêm tài khoản vào database
                _context.SystemAccounts.Add(account);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding account: {ex.Message}", ex);
            }
        }


        public async Task DeleteAccount(short id)
        {
            try
            {
                var existingAccount = await _context.SystemAccounts.FirstOrDefaultAsync(art => art.AccountId == id);
                if (existingAccount == null)
                {
                    throw new Exception("Account does not exist");
                }

                existingAccount.Status = !existingAccount.Status;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting account: {ex.Message}", ex);
            }
        }

        public async Task<SystemAccount> GetAccountById(short id)
        {
            return await _context.SystemAccounts.FirstOrDefaultAsync(art => art.AccountId == id);
        }



        public async Task<List<SystemAccount>> GetSystemAccounts()
        {
            return await _context.SystemAccounts.ToListAsync();
        }


        public async Task UpdateAccount(SystemAccount account)
        {

            var existingAccount = await _context.SystemAccounts.FindAsync(account.AccountId);
            if (existingAccount != null)
            {
                existingAccount.AccountName = account.AccountName;
                existingAccount.AccountEmail = account.AccountEmail;
                existingAccount.AccountRole = account.AccountRole;
                existingAccount.ImgUrl = account.ImgUrl;

                await _context.SaveChangesAsync();
            }
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
            return await _context.SystemAccounts.FirstOrDefaultAsync(x => x.AccountEmail == email && x.Status== true);
        }
    }
}
