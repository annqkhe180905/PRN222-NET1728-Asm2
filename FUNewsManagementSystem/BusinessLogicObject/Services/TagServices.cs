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
    public class TagServices : ITagServices
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        
        public TagServices(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }
    
        public void CreateTag(TagDTO tag)
        {
            var newsTag = _mapper.Map<Tag>(tag);
            _tagRepository.CreateTag(newsTag);

        }

        public void DeleteTag(int tagId)
        {
            
            _tagRepository.DeleteTag(tagId);
        }

        public List<TagDTO> GetTags()
        {
            var tags = _tagRepository.GetTags();
            return _mapper.Map<List<TagDTO>>(tags);
        }

        public void UpdateTag(TagDTO tag)
        {
            var existedTag = _mapper.Map<Tag>(tag);
            _tagRepository.UpdateTag(existedTag);
        }
    }
}
