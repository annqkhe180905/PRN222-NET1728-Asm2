using BusinessLogicLayer.DTOs;
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
        public Task<AccountDTO> Login(string email, string password);
        
    }
}
