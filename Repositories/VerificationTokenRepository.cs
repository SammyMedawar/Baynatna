using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class VerificationTokenRepository : Repository<VerificationToken>, IVerificationTokenRepository
    {
        public VerificationTokenRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<VerificationToken>> GetAllAsync()
        {
            return await _context.VerificationTokens
                .Include(vt => vt.IssuedToUser)
                .ToListAsync();
        }

        public override async Task<VerificationToken?> GetByIdAsync(int id)
        {
            return await _context.VerificationTokens
                .Include(vt => vt.IssuedToUser)
                .FirstOrDefaultAsync(vt => vt.Id == id);
        }

        public override async Task<IEnumerable<VerificationToken>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.VerificationTokens
                .Include(vt => vt.IssuedToUser)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<VerificationToken> AddAsync(VerificationToken entity)
        {
            await _context.VerificationTokens.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<VerificationToken> UpdateAsync(VerificationToken entity)
        {
            _context.VerificationTokens.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            var token = await _context.VerificationTokens.FindAsync(id);
            if (token != null)
            {
                _context.VerificationTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }

        public void Update(VerificationToken entity)
        {
            _context.VerificationTokens.Update(entity);
        }
    }
} 