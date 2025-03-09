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
        List<TagDTO> GetTags();
        void CreateTag(TagDTO tag);
        void UpdateTag(TagDTO tag);
        void DeleteTag(int tagId);
    }
}
