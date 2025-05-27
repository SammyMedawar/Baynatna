namespace Baynatna.Models
{
    public class ComplaintTag
    {
        public int ComplaintId { get; set; }
        public int TagId { get; set; }

        public Complaint Complaint { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
} 