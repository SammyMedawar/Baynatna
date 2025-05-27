using System;

namespace Baynatna.Models
{
    public class ComplaintVote
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }
        public int UserId { get; set; }
        public bool IsUpvote { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public Complaint Complaint { get; set; } = null!;
        public User User { get; set; } = null!;
    }
} 