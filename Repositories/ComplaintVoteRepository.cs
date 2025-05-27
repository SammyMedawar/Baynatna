using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class ComplaintVoteRepository : Repository<ComplaintVote>, IComplaintVoteRepository
    {
        public ComplaintVoteRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<ComplaintVote>> GetAllAsync()
        {
            return await _context.ComplaintVotes
                .Include(cv => cv.User)
                .Include(cv => cv.Complaint)
                .ToListAsync();
        }

        public override async Task<ComplaintVote?> GetByIdAsync(int id)
        {
            return await _context.ComplaintVotes
                .Include(cv => cv.User)
                .Include(cv => cv.Complaint)
                .FirstOrDefaultAsync(cv => cv.Id == id);
        }

        public override async Task<IEnumerable<ComplaintVote>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.ComplaintVotes
                .Include(cv => cv.User)
                .Include(cv => cv.Complaint)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<ComplaintVote> AddAsync(ComplaintVote entity)
        {
            await _context.ComplaintVotes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<ComplaintVote> UpdateAsync(ComplaintVote entity)
        {
            _context.ComplaintVotes.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            var vote = await _context.ComplaintVotes.FindAsync(id);
            if (vote != null)
            {
                _context.ComplaintVotes.Remove(vote);
                await _context.SaveChangesAsync();
            }
        }
    }
}