using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class NewsArticleServices : INewsArticleServices
    {
        private readonly INewArticleRepository _newsArticleRepository;
        private readonly IMapper _mapper;

        public NewsArticleServices(INewArticleRepository newsArticleRepository, IMapper mapper)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsArticleDTO>> GetAllActiveArticles()
        {
            var news = await _newsArticleRepository.GetAllActiveArticles();
            return _mapper.Map<List<NewsArticleDTO>>(news);
        }
    }
}
