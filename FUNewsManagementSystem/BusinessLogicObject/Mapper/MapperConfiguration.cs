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
            CreateMap<NewsArticle, NewsArticleDTO>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : "Uncategorized"))
    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.AccountName : "Unknown"))
    .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => new TagDTO
    {
        TagId = tag.TagId,
        TagName = tag.TagName,
        Note = tag.Note
    }).ToList()));

            CreateMap<NewsArticleDTO, NewsArticle>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(tag => new Tag
                {
                    TagId = tag.TagId,
                    TagName = tag.TagName,
                    Note = tag.Note
                }).ToList()));

        }   
    }
}
