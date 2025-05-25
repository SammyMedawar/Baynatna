using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class CommentVoteRepository : ICommentVoteRepository
    {
        private readonly BaynatnaContext _context;
        public CommentVoteRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<CommentVote?> GetByIdAsync(int id) => await _context.CommentVotes.FindAsync(id);

        public async Task<IEnumerable<CommentVote>> GetAllAsync() => await _context.CommentVotes.ToListAsync();

        public async Task AddAsync(CommentVote entity) => await _context.CommentVotes.AddAsync(entity);

        public void Update(CommentVote entity) => _context.CommentVotes.Update(entity);

        public void Delete(CommentVote entity) => _context.CommentVotes.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 