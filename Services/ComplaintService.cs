using Baynatna.Models;
using Baynatna.Repositories.Interfaces;
using Baynatna.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Baynatna.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly IComplaintRepository _complaintRepository;
        private readonly IComplaintVoteRepository _complaintVoteRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IComplaintAuditLogRepository _auditLogRepository;
        private readonly BaynatnaContext _context;

        public ComplaintService(
            IComplaintRepository complaintRepository,
            IComplaintVoteRepository complaintVoteRepository,
            ICommentRepository commentRepository,
            ITagRepository tagRepository,
            IComplaintAuditLogRepository auditLogRepository,
            BaynatnaContext context)
        {
            _complaintRepository = complaintRepository;
            _complaintVoteRepository = complaintVoteRepository;
            _commentRepository = commentRepository;
            _tagRepository = tagRepository;
            _auditLogRepository = auditLogRepository;
            _context = context;
        }

        public async Task<ServiceResult> CreateComplaintAsync(int userId, CreateComplaintViewModel model)
        {
            var complaint = new Complaint
            {
                ThreadId = Guid.NewGuid(),
                UserId = userId,
                OriginalTitle = model.OriginalTitle,
                OriginalBody = model.OriginalBody,
                TranslatedTitle = model.TranslatedTitle,
                TranslatedBody = model.TranslatedBody,
                VoiceMessageUrl = model.VoiceMessageUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _complaintRepository.AddAsync(complaint);

            if (model.TagIds != null && model.TagIds.Any())
            {
                var tags = await _tagRepository.GetAllAsync();
                var validTagIds = tags.Where(t => model.TagIds.Contains(t.Id)).Select(t => t.Id);
                foreach (var tagId in validTagIds)
                {
                    await _context.ComplaintTags.AddAsync(new ComplaintTag
                    {
                        ComplaintId = complaint.Id,
                        TagId = tagId
                    });
                }
                await _context.SaveChangesAsync();
            }

            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> DeleteComplaintAsync(int id, int userId)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return new ServiceResult { Success = false, ErrorMessage = "Complaint not found" };

            if (complaint.UserId != userId)
                return new ServiceResult { Success = false, ErrorMessage = "You can only delete your own complaints" };

            await _complaintRepository.DeleteAsync(id);
            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> VoteAsync(int userId, int complaintId, bool isUpvote, string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                return new ServiceResult { Success = false, ErrorMessage = "Vote reason is required" };

            var complaint = await _complaintRepository.GetByIdAsync(complaintId);
            if (complaint == null)
                return new ServiceResult { Success = false, ErrorMessage = "Complaint not found" };

            var existingVote = await _complaintVoteRepository.GetAllAsync();
            existingVote = existingVote.Where(v => v.ComplaintId == complaintId && v.UserId == userId);

            if (existingVote.Any())
            {
                var vote = existingVote.First();
                if (vote.IsUpvote == isUpvote)
                {
                    await _complaintVoteRepository.DeleteAsync(vote.Id);
                }
                else
                {
                    vote.IsUpvote = isUpvote;
                    vote.Reason = reason;
                    await _complaintVoteRepository.UpdateAsync(vote);
                }
            }
            else
            {
                await _complaintVoteRepository.AddAsync(new ComplaintVote
                {
                    ComplaintId = complaintId,
                    UserId = userId,
                    IsUpvote = isUpvote,
                    Reason = reason,
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> AddCommentAsync(int userId, AddCommentViewModel model)
        {
            var complaint = await _complaintRepository.GetByIdAsync(model.ComplaintId);
            if (complaint == null)
                return new ServiceResult { Success = false, ErrorMessage = "Complaint not found" };

            var comment = new Comment
            {
                ComplaintId = model.ComplaintId,
                UserId = userId,
                OriginalBody = model.Body,
                TranslatedBody = model.TranslatedBody,
                VoiceMessageUrl = model.VoiceMessageUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment);
            return new ServiceResult { Success = true };
        }

        public async Task<ServiceResult> DeleteCommentAsync(int id, int userId)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                return new ServiceResult { Success = false, ErrorMessage = "Comment not found" };

            if (comment.UserId != userId)
                return new ServiceResult { Success = false, ErrorMessage = "You can only delete your own comments" };

            await _commentRepository.DeleteAsync(id);
            return new ServiceResult { Success = true };
        }

        public async Task<Complaint?> GetComplaintDetailsAsync(int id, int? userId)
        {
            var complaint = await _complaintRepository.GetByIdAsync(id);
            if (complaint == null)
                return null;

            if (userId.HasValue)
            {
                var userVote = await _complaintVoteRepository.GetAllAsync();
                userVote = userVote.Where(v => v.ComplaintId == id && v.UserId == userId.Value);
                if (userVote.Any())
                {
                    var vote = userVote.First();
                    complaint.UserVote = vote.IsUpvote;
                    complaint.UserVoteReason = vote.Reason;
                }
            }

            return complaint;
        }

        public async Task<IEnumerable<Complaint>> GetComplaintsAsync(ComplaintListQuery query)
        {
            var complaints = await _complaintRepository.GetAllAsync();

            if (!query.ShowQuarantined)
                complaints = complaints.Where(c => !c.IsQuarantined);

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                complaints = complaints.Where(c =>
                    c.OriginalTitle.ToLower().Contains(searchTerm) ||
                    c.OriginalBody.ToLower().Contains(searchTerm) ||
                    c.TranslatedTitle != null && c.TranslatedTitle.ToLower().Contains(searchTerm) ||
                    c.TranslatedBody != null && c.TranslatedBody.ToLower().Contains(searchTerm));
            }

            if (query.TagId.HasValue)
                complaints = complaints.Where(c => c.ComplaintTags.Any(ct => ct.TagId == query.TagId));

            complaints = query.SortBy?.ToLower() switch
            {
                "date" => query.SortDescending
                    ? complaints.OrderByDescending(c => c.CreatedAt)
                    : complaints.OrderBy(c => c.CreatedAt),
                "likes" => query.SortDescending
                    ? complaints.OrderByDescending(c => c.ComplaintVotes.Count(v => v.IsUpvote))
                    : complaints.OrderBy(c => c.ComplaintVotes.Count(v => v.IsUpvote)),
                "comments" => query.SortDescending
                    ? complaints.OrderByDescending(c => c.Comments.Count)
                    : complaints.OrderBy(c => c.Comments.Count),
                _ => complaints.OrderByDescending(c => c.CreatedAt)
            };

            return complaints.Skip((query.Page - 1) * query.PageSize)
                           .Take(query.PageSize);
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _tagRepository.GetAllAsync();
        }
    }
} 