using System;

namespace Baynatna.Models
{
    public class VerificationToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public bool IsUsed { get; set; }
        public int? IssuedToUserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public User? IssuedToUser { get; set; }
    }
} 