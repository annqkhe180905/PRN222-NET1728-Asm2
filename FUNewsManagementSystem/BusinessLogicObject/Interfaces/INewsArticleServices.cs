using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface INewsArticleServices 
    {
        Task<IEnumerable<NewsArticleDTO>> GetAllActiveArticles();

    }
}
