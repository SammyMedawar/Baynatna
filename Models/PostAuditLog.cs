using System;

namespace Baynatna.Models
{
    public class PostAuditLog
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int ModeratorId { get; set; }
        public string Action { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public Post Post { get; set; } = null!;
        public User Moderator { get; set; } = null!;
    }
} 