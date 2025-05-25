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
        Task<ServiceResult> VoteAsync(int userId, int postId, bool isUpvote, string reason);
        Task<PostDetailsViewModel?> GetPostDetailsAsync(int postId, int? userId);
        Task<ServiceResult> DeletePostAsync(int postId, int userId);
        Task<ServiceResult> AddCommentAsync(int userId, AddCommentViewModel model);
        Task<ServiceResult> DeleteCommentAsync(int commentId, int userId);
        Task<ServiceResult> VoteCommentAsync(int userId, int commentId, bool isUpvote);
        Task<List<CommentViewModel>> GetCommentsAsync(int postId, int? userId);
    }
} 