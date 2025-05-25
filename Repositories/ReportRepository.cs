using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly BaynatnaContext _context;
        public ReportRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<Report?> GetByIdAsync(int id) => await _context.Reports.FindAsync(id);

        public async Task<IEnumerable<Report>> GetAllAsync() => await _context.Reports.ToListAsync();

        public async Task AddAsync(Report entity) => await _context.Reports.AddAsync(entity);

        public void Update(Report entity) => _context.Reports.Update(entity);

        public void Delete(Report entity) => _context.Reports.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 