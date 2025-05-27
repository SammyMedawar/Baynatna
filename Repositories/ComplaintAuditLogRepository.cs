using Baynatna.Models;
using Baynatna.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Baynatna.Repositories
{
    public class ComplaintAuditLogRepository : Repository<ComplaintAuditLog>, IComplaintAuditLogRepository
    {
        public ComplaintAuditLogRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<ComplaintAuditLog?> GetByIdAsync(int id)
        {
            return await _context.ComplaintAuditLogs
                .Include(l => l.Complaint)
                .Include(l => l.Moderator)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
} 