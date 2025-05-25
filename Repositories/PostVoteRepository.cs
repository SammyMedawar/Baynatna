using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class PostVoteRepository : IPostVoteRepository
    {
        private readonly BaynatnaContext _context;
        public PostVoteRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<PostVote?> GetByIdAsync(int id) => await _context.PostVotes.FindAsync(id);

        public async Task<IEnumerable<PostVote>> GetAllAsync() => await _context.PostVotes.ToListAsync();

        public async Task AddAsync(PostVote entity) => await _context.PostVotes.AddAsync(entity);

        public void Update(PostVote entity) => _context.PostVotes.Update(entity);

        public void Delete(PostVote entity) => _context.PostVotes.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 