using System;
using System.Collections.Generic;

namespace Baynatna.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public int UserId { get; set; }
        public string OriginalBody { get; set; } = null!;
        public string? TranslatedBody { get; set; }
        public bool IsTranslatedByModerator { get; set; }
        public string? VoiceMessageUrl { get; set; }
        public bool IsDeletedByModerator { get; set; }
        public DateTime CreatedAt { get; set; }

        public Complaint Complaint { get; set; } = null!;
        public User User { get; set; } = null!;
        public ICollection<CommentVote>? CommentVotes { get; set; }
    }
} 