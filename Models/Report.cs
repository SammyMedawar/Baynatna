using System;

namespace Baynatna.Models
{
    public class Report
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public int? ComplaintId { get; set; }
        public int? CommentId { get; set; }
        public string Reason { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public User Reporter { get; set; } = null!;
        public Complaint? Complaint { get; set; }
        public Comment? Comment { get; set; }
    }
} 