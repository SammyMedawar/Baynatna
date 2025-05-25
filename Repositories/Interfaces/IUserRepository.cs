using Baynatna.Models;

namespace Baynatna.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        // Add user-specific methods here if needed
        Task<User?> GetByUsernameAsync(string username);
    }
} 