using AutoMapper;
using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Mapper
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration() 
        {
            CreateMap<SystemAccount, AccountDTO>().ReverseMap();
            CreateMap<Category,CategoryDTO>().ReverseMap();
            CreateMap<Tag,TagDTO>().ReverseMap();
            CreateMap<NewsArticle,NewsArticleDTO>().ReverseMap();

        }   
    }
}
