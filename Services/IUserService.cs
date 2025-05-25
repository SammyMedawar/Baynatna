using System.Threading.Tasks;
using Baynatna.ViewModels;

namespace Baynatna.Services
{
    public interface IUserService
    {
        Task<ServiceResult> LoginAsync(string username, string password);
        Task<ServiceResult> RegisterAsync(string username, string password, string confirmPassword, string token);
        Task<ServiceResult> RequestTokenAsync(string phone, string email, string idOrProof);
    }
} 