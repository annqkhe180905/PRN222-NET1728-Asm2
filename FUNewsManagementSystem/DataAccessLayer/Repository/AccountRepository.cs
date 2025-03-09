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
        private readonly FunewsManagementContext _dbContext;
        public AccountRepository(FunewsManagementContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAcount(SystemAccount account)
        {
            try
            {
                // Kiểm tra xem email đã tồn tại hay chưa
                bool emailExists = await _dbContext.SystemAccounts.AnyAsync(a => a.AccountEmail == account.AccountEmail);
                if (emailExists)
                {
                    throw new Exception("Email already exists");
                }

                // Tạo ID mới dựa trên số lượng tài khoản hiện có
                account.AccountId = (short)(await _dbContext.SystemAccounts.CountAsync() + 1);
                account.AccountPassword = "@1";
                // Thêm tài khoản vào database
                _dbContext.SystemAccounts.Add(account);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding account: {ex.Message}", ex);
            }
        }





        public async Task DeleteAcount(short id)
        {
            try
            {
                var exitAccount = await _dbContext.SystemAccounts.FirstOrDefaultAsync(art => art.AccountId == id);
                if (exitAccount == null)
                {
                    throw new Exception("Account not exist");
                }

                exitAccount.AccountStatus = false;

                await _dbContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SystemAccount> GetAcountById(short id)
        {
            return await _dbContext.SystemAccounts.FirstOrDefaultAsync(art => art.AccountId == id);

        }

        public Task<SystemAccount> GetListPagingAcounts(string searchTerm, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SystemAccount>> GetSystemAccounts()
        {
            return await _dbContext.SystemAccounts.ToListAsync();
        }

        public async Task UpdateAcount(SystemAccount account)
        {

            bool emailExists = await _dbContext.SystemAccounts.AnyAsync(a => a.AccountEmail == account.AccountEmail);
            if (emailExists)
            {
                throw new Exception("Email already exists");
            }
            var existingAccount = await _dbContext.SystemAccounts.FindAsync(account.AccountId);

    
            if (existingAccount != null)
            {
                existingAccount.AccountName = account.AccountName;
                existingAccount.AccountEmail = account.AccountEmail;
                existingAccount.AccountRole = account.AccountRole;
              //  existingAccount.ImgUrl = account.ImgUrl;
                existingAccount.AccountStatus = account.AccountStatus;

           

                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
