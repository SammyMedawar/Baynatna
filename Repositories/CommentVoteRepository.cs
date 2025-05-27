using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class CommentVoteRepository : Repository<CommentVote>, ICommentVoteRepository
    {
        public CommentVoteRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<CommentVote>> GetAllAsync()
        {
            return await _context.CommentVotes
                .Include(cv => cv.User)
                .Include(cv => cv.Comment)
                .ToListAsync();
        }

        public override async Task<CommentVote?> GetByIdAsync(int id)
        {
            return await _context.CommentVotes
                .Include(cv => cv.User)
                .Include(cv => cv.Comment)
                .FirstOrDefaultAsync(cv => cv.Id == id);
        }

        public override async Task<IEnumerable<CommentVote>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.CommentVotes
                .Include(cv => cv.User)
                .Include(cv => cv.Comment)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<CommentVote> AddAsync(CommentVote entity)
        {
            await _context.CommentVotes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<CommentVote> UpdateAsync(CommentVote entity)
        {
            _context.CommentVotes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            var vote = await _context.CommentVotes.FindAsync(id);
            if (vote != null)
            {
                _context.CommentVotes.Remove(vote);
                await _context.SaveChangesAsync();
            }
        }
    }
} 