using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags
                .Include(t => t.ComplaintTags)
                .ToListAsync();
        }

        public override async Task<Tag?> GetByIdAsync(int id)
        {
            return await _context.Tags
                .Include(t => t.ComplaintTags)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<IEnumerable<Tag>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Tags
                .Include(t => t.ComplaintTags)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<Tag> AddAsync(Tag entity)
        {
            await _context.Tags.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<Tag> UpdateAsync(Tag entity)
        {
            _context.Tags.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            var tag = await _context.Tags.FindAsync(id);
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();
            }
        }
    }
} 