using global::Messaging.Core.Models;
// Em Messaging.Infrastructure/Data/MessageDbContext.cs
using Microsoft.EntityFrameworkCore;

namespace Messaging.Infrastructure.Data
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options)
            : base(options) { }

        public DbSet<MessageLog> MessageLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Recipient).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Subject).HasMaxLength(500);
                entity.Property(e => e.SentAt).IsRequired();
                entity.Property(e => e.Status).IsRequired();
            });
        }
    }
}

