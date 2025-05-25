using System.Threading.Tasks;
using Baynatna.ViewModels;

namespace Baynatna.Services
{
    public interface IPostService
    {
        Task<ServiceResult> CreatePostAsync(int userId, CreatePostViewModel model);
    }
} 