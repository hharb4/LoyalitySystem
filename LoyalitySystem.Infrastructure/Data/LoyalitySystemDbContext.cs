using LoyalitySystem.Domain;
using Microsoft.EntityFrameworkCore;
using System;
namespace LoyalitySystem.Infrastructure.Data
{
    public class LoyalitySystemDbContext : DbContext
    {
        public LoyalitySystemDbContext(DbContextOptions<LoyalitySystemDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Point> Points { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Points)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);
        }
    }
}
