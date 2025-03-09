using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Interfaces;
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

        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagServices(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDTO>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllTagsAsync();
            return _mapper.Map<List<TagDTO>>(tags);
        }
    }
}
