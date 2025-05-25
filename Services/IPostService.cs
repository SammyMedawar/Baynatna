using System.Threading.Tasks;
using Baynatna.ViewModels;
using System.Collections.Generic;

namespace Baynatna.Services
{
    public interface IPostService
    {
        Task<ServiceResult> CreatePostAsync(int userId, CreatePostViewModel model);
        Task<PostListResult> GetPostsAsync(PostListQuery query);
        Task<List<TagViewModel>> GetAllTagsAsync();
    }
} 