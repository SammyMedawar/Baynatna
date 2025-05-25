using System;
using System.Collections.Generic;

namespace Baynatna.Models
{
    public class Post
    {
        public int Id { get; set; }
        public Guid ThreadId { get; set; }
        public int UserId { get; set; }
        public string OriginalBody { get; set; } = null!;
        public string? TranslatedBody { get; set; }
        public bool IsTranslatedByModerator { get; set; }
        public string? VoiceMessageUrl { get; set; }
        public bool IsQuarantined { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OriginalTitle { get; set; } = null!;
        public string? TranslatedTitle { get; set; }

        public User User { get; set; } = null!;
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostVote>? PostVotes { get; set; }
        public ICollection<PostTag>? PostTags { get; set; }
        public ICollection<PostAuditLog>? AuditLogs { get; set; }
    }
} 