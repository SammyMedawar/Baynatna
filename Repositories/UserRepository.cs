using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Baynatna.Models;
using Baynatna.Repositories.Interfaces;

namespace Baynatna.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(BaynatnaContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.Complaints)
                .Include(u => u.Comments)
                .Include(u => u.ComplaintVotes)
                .Include(u => u.CommentVotes)
                .ToListAsync();
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Complaints)
                .Include(u => u.Comments)
                .Include(u => u.ComplaintVotes)
                .Include(u => u.CommentVotes)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public override async Task<IEnumerable<User>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Users
                .Include(u => u.Complaints)
                .Include(u => u.Comments)
                .Include(u => u.ComplaintVotes)
                .Include(u => u.CommentVotes)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public override async Task<User> AddAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task<User> UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByUsernameAsync(string username) => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
} 