using BusinessLogicLayer.DTOs;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ITagServices
    {
        Task<List<TagDTO>> GetTags();
        Task<int> CreateTag(TagDTO tag);
        Task<bool> UpdateTag(TagDTO tag);
        Task<bool> DeleteTag(int tagId);
    }
}
