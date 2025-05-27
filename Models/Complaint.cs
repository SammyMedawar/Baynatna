using System;
using System.Collections.Generic;
using System.Linq;

namespace Baynatna.Models
{
    public class Complaint
    {
        public int Id { get; set; }
        public Guid ThreadId { get; set; }
        public int UserId { get; set; }
        public string OriginalTitle { get; set; } = null!;
        public string OriginalBody { get; set; } = null!;
        public string? TranslatedTitle { get; set; }
        public string? TranslatedBody { get; set; }
        public bool IsTranslatedByModerator { get; set; }
        public string? VoiceMessageUrl { get; set; }
        public bool IsQuarantined { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<ComplaintVote> ComplaintVotes { get; set; } = new List<ComplaintVote>();
        public ICollection<ComplaintTag> ComplaintTags { get; set; } = new List<ComplaintTag>();

        // Computed properties
        public int Upvotes => ComplaintVotes?.Count(v => v.IsUpvote) ?? 0;
        public int Downvotes => ComplaintVotes?.Count(v => !v.IsUpvote) ?? 0;
        public string Title => OriginalTitle;
        public string Body => OriginalBody;

        // User-specific properties
        public bool? UserVote { get; set; }
        public string? UserVoteReason { get; set; }
    }
} 