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

        public async Task<int> CreateTag(TagDTO tag)
        {
            if (tag == null || string.IsNullOrWhiteSpace(tag.TagName))
            {
                throw new ArgumentException("Tag data is invalid.");
            }

            var newTag = _mapper.Map<Tag>(tag);
            return await _tagRepository.CreateTag(newTag);
        }

        public async Task<bool> DeleteTag(int tagId)
        {
            if (tagId < 0)
            {
                throw new ArgumentException("Invalid tag ID.");
            }

            return await _tagRepository.DeleteTag(tagId);
        }

        public async Task<List<TagDTO>> GetTags()
        {
            var tags = await _tagRepository.GetTags();
            return _mapper.Map<List<TagDTO>>(tags);
        }

        public async Task<bool> UpdateTag(TagDTO tag)
        {
            if (tag == null || tag.TagId <= 0 || string.IsNullOrWhiteSpace(tag.TagName))
            {
                throw new ArgumentException("Invalid tag data.");
            }

            var existingTag = _mapper.Map<Tag>(tag);
            return await _tagRepository.UpdateTag(existingTag);
        }
    }

}
