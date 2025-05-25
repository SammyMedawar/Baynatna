using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Baynatna.Models
{
    public class BaynatnaContext : DbContext
    {
        public BaynatnaContext(DbContextOptions<BaynatnaContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<VerificationToken> VerificationTokens { get; set; } = null!;
        public DbSet<Post> Posts { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<PostVote> PostVotes { get; set; } = null!;
        public DbSet<CommentVote> CommentVotes { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<PostTag> PostTags { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<PostAuditLog> PostAuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique constraints
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<VerificationToken>().HasIndex(t => t.Token).IsUnique();
            modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

            // Composite keys
            modelBuilder.Entity<PostTag>().HasKey(pt => new { pt.PostId, pt.TagId });
            modelBuilder.Entity<PostVote>().HasIndex(v => new { v.PostId, v.UserId }).IsUnique();
            modelBuilder.Entity<CommentVote>().HasIndex(v => new { v.CommentId, v.UserId }).IsUnique();

            // Relationships
            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);
            modelBuilder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }
    }
}