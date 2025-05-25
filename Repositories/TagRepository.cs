using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly BaynatnaContext _context;
        public TagRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<Tag?> GetByIdAsync(int id) => await _context.Tags.FindAsync(id);

        public async Task<IEnumerable<Tag>> GetAllAsync() => await _context.Tags.ToListAsync();

        public async Task AddAsync(Tag entity) => await _context.Tags.AddAsync(entity);

        public void Update(Tag entity) => _context.Tags.Update(entity);

        public void Delete(Tag entity) => _context.Tags.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 