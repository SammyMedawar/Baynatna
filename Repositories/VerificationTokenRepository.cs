using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class VerificationTokenRepository : IVerificationTokenRepository
    {
        private readonly BaynatnaContext _context;
        public VerificationTokenRepository(BaynatnaContext context)
        {
            _context = context;
        }

        public async Task<VerificationToken?> GetByIdAsync(int id) => await _context.VerificationTokens.FindAsync(id);

        public async Task<IEnumerable<VerificationToken>> GetAllAsync() => await _context.VerificationTokens.ToListAsync();

        public async Task AddAsync(VerificationToken entity) => await _context.VerificationTokens.AddAsync(entity);

        public void Update(VerificationToken entity) => _context.VerificationTokens.Update(entity);

        public void Delete(VerificationToken entity) => _context.VerificationTokens.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
} 