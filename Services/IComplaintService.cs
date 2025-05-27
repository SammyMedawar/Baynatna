using Baynatna.Models;
using Baynatna.ViewModels;

namespace Baynatna.Services
{
    public interface IComplaintService
    {
        Task<ServiceResult> CreateComplaintAsync(int userId, CreateComplaintViewModel model);
        Task<ServiceResult> DeleteComplaintAsync(int id, int userId);
        Task<ServiceResult> VoteAsync(int userId, int complaintId, bool isUpvote, string reason);
        Task<ServiceResult> AddCommentAsync(int userId, AddCommentViewModel model);
        Task<ServiceResult> DeleteCommentAsync(int id, int userId);
        Task<Complaint?> GetComplaintDetailsAsync(int id, int? userId);
        Task<IEnumerable<Complaint>> GetComplaintsAsync(ComplaintListQuery query);
        Task<IEnumerable<Tag>> GetAllTagsAsync();
    }
} 