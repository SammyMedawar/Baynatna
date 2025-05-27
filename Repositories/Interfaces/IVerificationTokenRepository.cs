using Baynatna.Models;

namespace Baynatna.Repositories.Interfaces
{
    public interface IVerificationTokenRepository : IRepository<VerificationToken>
    {
        void Update(VerificationToken entity);
    }
} 