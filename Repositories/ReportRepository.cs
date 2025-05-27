using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Complaint)
                .Include(r => r.Comment)
                .ToListAsync();
        }

        public override async Task<Report?> GetByIdAsync(int id)
        {
            return await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Complaint)
                .Include(r => r.Comment)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public override async Task<IEnumerable<Report>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Reports
                .Include(r => r.Reporter)
                .Include(r => r.Complaint)
                .Include(r => r.Comment)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<Report> AddAsync(Report entity)
        {
            await _context.Reports.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<Report> UpdateAsync(Report entity)
        {
            _context.Reports.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report != null)
            {
                _context.Reports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
} 