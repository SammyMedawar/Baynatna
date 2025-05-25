using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly BaynatnaContext _context;
        public PostRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<Post?> GetByIdAsync(int id) => await _context.Posts.FindAsync(id);

        public async Task<IEnumerable<Post>> GetAllAsync() => await _context.Posts.ToListAsync();

        public async Task AddAsync(Post entity) => await _context.Posts.AddAsync(entity);

        public void Update(Post entity) => _context.Posts.Update(entity);

        public void Delete(Post entity) => _context.Posts.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 