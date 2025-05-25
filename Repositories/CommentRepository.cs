using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BaynatnaContext _context;
        public CommentRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetByIdAsync(int id) => await _context.Comments.FindAsync(id);

        public async Task<IEnumerable<Comment>> GetAllAsync() => await _context.Comments.ToListAsync();

        public async Task AddAsync(Comment entity) => await _context.Comments.AddAsync(entity);

        public void Update(Comment entity) => _context.Comments.Update(entity);

        public void Delete(Comment entity) => _context.Comments.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 