using Microsoft.EntityFrameworkCore;
using SubscripSync.Domain.Entities;

namespace SubscripSync.Infrastructure.Data
{
    public class SubscripSyncDbContext : DbContext
    {
        public SubscripSyncDbContext(DbContextOptions<SubscripSyncDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<PaymentHistory> PaymentHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subscription>()
                .Property(s => s.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<PaymentHistory>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
