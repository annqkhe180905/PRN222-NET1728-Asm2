using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface ITagRepository
    {
        Task<int> CreateTag(Tag tag);
        Task<bool> DeleteTag(int tagId);
        Task<List<Tag>> GetTags();
        Task<bool> UpdateTag(Tag tag);
        Task<int> CountAsync ();
        Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds);
    }
}
