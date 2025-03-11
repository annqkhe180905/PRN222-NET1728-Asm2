using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly FunewsManagementContext _context;

        public TagRepository(FunewsManagementContext context)
        {
            _context = context;
        }

        public async Task<int> CountAsync()
        {
            return await _context.Tags.AsNoTracking().CountAsync();
        }

        public async Task<int> CreateTag(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            tag.TagId = (int)(await _context.Tags.CountAsync() + 1);
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag.TagId; 
        }

        public async Task<bool> DeleteTag(int tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null) return false;

            _context.Tags.Remove(tag);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Tag>> GetTags()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<bool> UpdateTag(Tag tag)
        {
            var existingTag = await _context.Tags.FindAsync(tag.TagId);
            if (existingTag == null) return false;

            existingTag.TagName = tag.TagName;
            existingTag.Note = tag.Note;

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<List<Tag>> GetTagsByIdsAsync(List<int> tagIds)
        {
            return await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .ToListAsync();
        }


    }

}
