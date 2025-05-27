using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Baynatna.Models
{
    public class BaynatnaContext : DbContext
    {
        public BaynatnaContext(DbContextOptions<BaynatnaContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ComplaintVote> ComplaintVotes { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ComplaintTag> ComplaintTags { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ComplaintAuditLog> ComplaintAuditLogs { get; set; }
        public DbSet<VerificationToken> VerificationTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<VerificationToken>()
                .HasIndex(vt => vt.Token)
                .IsUnique();

            modelBuilder.Entity<ComplaintVote>()
                .HasIndex(cv => new { cv.ComplaintId, cv.UserId })
                .IsUnique();

            modelBuilder.Entity<CommentVote>()
                .HasIndex(cv => new { cv.CommentId, cv.UserId })
                .IsUnique();

            modelBuilder.Entity<ComplaintTag>()
                .HasKey(ct => new { ct.ComplaintId, ct.TagId });

            // Relationships
            modelBuilder.Entity<ComplaintTag>()
                .HasOne(ct => ct.Complaint)
                .WithMany(c => c.ComplaintTags)
                .HasForeignKey(ct => ct.ComplaintId);
            modelBuilder.Entity<ComplaintTag>()
                .HasOne(ct => ct.Tag)
                .WithMany(t => t.ComplaintTags)
                .HasForeignKey(ct => ct.TagId);
        }
    }
}