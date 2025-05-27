using System;

namespace Baynatna.Models
{
    public class ComplaintAuditLog
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public int ModeratorId { get; set; }
        public string Action { get; set; } = null!;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        public Complaint Complaint { get; set; } = null!;
        public User Moderator { get; set; } = null!;
    }
} 