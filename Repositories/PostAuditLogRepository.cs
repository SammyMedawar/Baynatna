using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class PostAuditLogRepository : IPostAuditLogRepository
    {
        private readonly BaynatnaContext _context;
        public PostAuditLogRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<PostAuditLog?> GetByIdAsync(int id) => await _context.PostAuditLogs.FindAsync(id);

        public async Task<IEnumerable<PostAuditLog>> GetAllAsync() => await _context.PostAuditLogs.ToListAsync();

        public async Task AddAsync(PostAuditLog entity) => await _context.PostAuditLogs.AddAsync(entity);

        public void Update(PostAuditLog entity) => _context.PostAuditLogs.Update(entity);

        public void Delete(PostAuditLog entity) => _context.PostAuditLogs.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 