using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class ComplaintRepository : Repository<Complaint>, IComplaintRepository
    {
        public ComplaintRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Complaint>> GetAllAsync()
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Comments)
                .Include(c => c.ComplaintVotes)
                .Include(c => c.ComplaintTags)
                .ToListAsync();
        }

        public override async Task<Complaint?> GetByIdAsync(int id)
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Comments)
                .Include(c => c.ComplaintVotes)
                .Include(c => c.ComplaintTags)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<IEnumerable<Complaint>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Complaints
                .Include(c => c.User)
                .Include(c => c.Comments)
                .Include(c => c.ComplaintVotes)
                .Include(c => c.ComplaintTags)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<Complaint> AddAsync(Complaint entity)
        {
            await _context.Complaints.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<Complaint> UpdateAsync(Complaint entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
                await _context.SaveChangesAsync();
            }
        }

        public override async Task<bool> ExistsAsync(int id)
        {
            return await _context.Complaints.AnyAsync(c => c.Id == id);
        }

        public override async Task<int> CountAsync()
        {
            return await _context.Complaints.CountAsync();
        }
    }
} 